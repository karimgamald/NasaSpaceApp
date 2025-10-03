using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharkTracking.Application.DTOs;
using SharkTracking.InfrastructureData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class HeatmapController : ControllerBase
{
    private readonly NasaDbContext _context;
    public HeatmapController(NasaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// GET /api/heatmap?from=2025-09-01&to=2025-09-30&top=200
    /// Returns points with intensity score for rendering on heatmap.
    /// Intensity = weighted sum of shark activity density + satellite indicators.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HeatmapDTO>>> GetHeatmap(
        DateTime? from = null,
        DateTime? to = null,
        int top = 200)
    {
        var fromDt = from ?? DateTime.UtcNow.AddDays(-7);
        var toDt = to ?? DateTime.UtcNow;

        // 1) Shark tag activity points
        var sharkPoints = await _context.SharkTagData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .Select(s => new
            {
                LatKey = Math.Round(s.Latitude, 3),
                LonKey = Math.Round(s.Longitude, 3),
                s.Latitude,
                s.Longitude,
                Count = 1,
                s.Chlorophyll,
                WaterTemp = s.WaterTemperature
            })
            .ToListAsync();

        var sharkGroups = sharkPoints
            .GroupBy(p => (p.LatKey, p.LonKey))
            .Select(g => new
            {
                g.Key.LatKey,
                g.Key.LonKey,
                Latitude = g.Average(x => x.Latitude),
                Longitude = g.Average(x => x.Longitude),
                Count = g.Count(),
                AvgChl = g.Average(x => x.Chlorophyll),
                AvgTemp = g.Average(x => x.WaterTemp)
            })
            .ToList();

        // 2) Satellite data points
        var satPoints = await _context.SatelliteData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .Select(s => new
            {
                LatKey = Math.Round(s.Latitude, 3),
                LonKey = Math.Round(s.Longitude, 3),
                s.Latitude,
                s.Longitude,
                s.ChlorophyllConcentration,
                s.SeaSurfaceTemperature,
                s.OxygenLevel
            })
            .ToListAsync();

        var satGroups = satPoints
            .GroupBy(p => (p.LatKey, p.LonKey))
            .Select(g => new
            {
                g.Key.LatKey,
                g.Key.LonKey,
                Latitude = g.Average(x => x.Latitude),
                Longitude = g.Average(x => x.Longitude),
                AvgChl = g.Average(x => x.ChlorophyllConcentration),
                AvgSst = g.Average(x => x.SeaSurfaceTemperature),
                AvgOxy = g.Average(x => x.OxygenLevel)
            })
            .ToList();

        // 3) Normalize helper
        double Normalize(double value, double min, double max) =>
            (max - min) < 1e-9 ? 0 : Math.Clamp((value - min) / (max - min), 0, 1);

        // ranges
        var counts = sharkGroups.Select(g => (double)g.Count).ToList();
        var chlValues = satGroups.Select(g => g.AvgChl).Concat(sharkGroups.Select(g => g.AvgChl)).ToList();
        var tempValues = satGroups.Select(g => g.AvgSst).Concat(sharkGroups.Select(g => g.AvgTemp)).ToList();

        double cMin = counts.Any() ? counts.Min() : 0;
        double cMax = counts.Any() ? counts.Max() : 1;
        double chlMin = chlValues.Any() ? chlValues.Min() : 0;
        double chlMax = chlValues.Any() ? Math.Max(chlValues.Max(), 1) : 1;
        double tMin = tempValues.Any() ? tempValues.Min() : 0;
        double tMax = tempValues.Any() ? Math.Max(tempValues.Max(), 1) : 1;

        var merged = new Dictionary<(double, double), (double lat, double lon, double intensity)>();

        // 4) Merge shark groups
        foreach (var g in sharkGroups)
        {
            var key = (g.LatKey, g.LonKey);
            var normCount = Normalize(g.Count, cMin, cMax);
            var normChl = Normalize(g.AvgChl, chlMin, chlMax);
            var normTemp = Normalize(g.AvgTemp, tMin, tMax);

            var intensity = 0.6 * normCount + 0.25 * normChl + 0.15 * normTemp;
            merged[key] = (g.Latitude, g.Longitude, intensity);
        }

        // 5) Merge satellite groups
        foreach (var s in satGroups)
        {
            var key = (s.LatKey, s.LonKey);
            var normChl = Normalize(s.AvgChl, chlMin, chlMax);
            var normTemp = Normalize(s.AvgSst, tMin, tMax);
            var intensity = 0.5 * normChl + 0.5 * normTemp;

            if (merged.TryGetValue(key, out var existing))
            {
                merged[key] = (existing.lat, existing.lon, (existing.intensity + intensity) / 2.0);
            }
            else
            {
                merged[key] = (s.Latitude, s.Longitude, intensity);
            }
        }

        // 6) Final result
        var result = merged.Values
            .Select(v => new HeatmapDTO
            {
                Lat = v.lat,
                Lng = v.lon,
                Intensity = Math.Round(v.intensity, 4)
            })
            .OrderByDescending(x => x.Intensity)
            .Take(top)
            .ToList();

        return Ok(result);
    }
}
