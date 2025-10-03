using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharkTracking.Application.DTOs;
using SharkTracking.InfrastructureData.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly NasaDbContext _context;
    public DashboardController(NasaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Dashboard Summary (cards + map + alerts)
    /// GET /api/dashboard/summary?from=2025-09-01&to=2025-09-30&topHotspots=10
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDTO>> GetSummary(DateTime? from = null, DateTime? to = null, int topHotspots = 10)
    {
        var fromDt = from ?? DateTime.UtcNow.AddDays(-7);
        var toDt = to ?? DateTime.UtcNow;

        // Total sharks
        var totalSharks = await _context.Sharks.CountAsync();

        // Active tags
        var activeTagCount = await _context.SharkTagData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .Select(s => s.SharkId)
            .Distinct()
            .CountAsync();

        // Average SST + Oxygen
        var avgSst = await _context.SatelliteData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .AverageAsync(s => (double?)s.SeaSurfaceTemperature) ?? 0;

        var avgOxy = await _context.SatelliteData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .AverageAsync(s => (double?)s.OxygenLevel) ?? 0;

        // Hotspots (for Live Map)
        var hotspots = await _context.SharkTagData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .GroupBy(s => new { LatKey = Math.Round(s.Latitude, 3), LonKey = Math.Round(s.Longitude, 3) })
            .Select(g => new
            {
                Lat = g.Average(x => x.Latitude),
                Lon = g.Average(x => x.Longitude),
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(topHotspots)
            .ToListAsync();

        var hotspotsDto = hotspots
            .Select(h => new HeatmapDTO
            {
                Lat = h.Lat,
                Lng = h.Lon,
                Intensity = h.Count
            })
            .ToList();

        // Recent Alerts
        var recentAlerts = await _context.PredictionAlerts
            .Where(a => a.Timestamp >= fromDt && a.Timestamp <= toDt)
            .OrderByDescending(a => a.Timestamp)
            .Take(20)
            .Select(a => new PredictionAlertDTO
            {
                Id = a.Id,
                SharkId = a.SharkId,
                Timestamp = a.Timestamp,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                PredictionType = a.PredictionType,
                AlertMessage = a.AlertMessage,
                RiskLevel = a.RiskLevel
            })
            .ToListAsync();

        var dto = new DashboardSummaryDTO
        {
            TotalSharks = totalSharks,
            ActiveTags = activeTagCount,
            AvgSeaSurfaceTemperature = Math.Round(avgSst, 3),
            AvgOxygenLevel = Math.Round(avgOxy, 3),
            TopHotspots = hotspotsDto,
            RecentAlerts = recentAlerts
        };

        return Ok(dto);
    }

    /// <summary>
    /// Shark detail (time series + alerts)
    /// GET /api/dashboard/shark/{id}?from=...&to=...
    /// </summary>
    [HttpGet("shark/{id}")]
    public async Task<ActionResult<SharkDetailDTO>> GetSharkDetail(int id, DateTime? from = null, DateTime? to = null)
    {
        var fromDt = from ?? DateTime.UtcNow.AddDays(-7);
        var toDt = to ?? DateTime.UtcNow;

        var shark = await _context.Sharks.FindAsync(id);
        if (shark == null) return NotFound();

        var tagSeries = await _context.SharkTagData
            .Where(s => s.SharkId == id && s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .OrderBy(s => s.Timestamp)
            .Select(s => new TimeSeriesPointDTO
            {
                Timestamp = s.Timestamp,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Value = s.Depth
            })
            .ToListAsync();

        var alerts = await _context.PredictionAlerts
            .Where(a => a.SharkId == id && a.Timestamp >= fromDt && a.Timestamp <= toDt)
            .OrderByDescending(a => a.Timestamp)
            .Select(a => new PredictionAlertDTO
            {
                Id = a.Id,
                SharkId = a.SharkId,
                Timestamp = a.Timestamp,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                PredictionType = a.PredictionType,
                AlertMessage = a.AlertMessage,
                RiskLevel = a.RiskLevel
            })
            .ToListAsync();
        var timeLapseData = await _context.SharkMedia
    .Where(m => m.SharkId == id && m.Timestamp >= fromDt && m.Timestamp <= toDt)
    .OrderBy(m => m.Timestamp)
    .Select(m => new TimeLapseDTO
    {
        Timestamp = m.Timestamp,
        MediaUrl = m.MediaUrl,
        MediaType = m.MediaType
    })
    .ToListAsync();

        var sharkDetails = new SharkDetailDTO
        {
            SharkId = shark.Id,
            Name = shark.Name,
            TagTimeSeries = tagSeries, // بيانات الحساسات الأخرى
            Alerts = alerts
        };


        var dto = new SharkDetailDTO
        {
            SharkId = shark.Id,
            Name = shark.Name,
            TagTimeSeries = tagSeries,
            Alerts = alerts
        };

        return Ok(dto);
    }

    /// <summary>
    /// Latest Tag Updates (table in dashboard)
    /// GET /api/dashboard/latest-tags
    /// </summary>
    [HttpGet("latest-tags")]
    public async Task<ActionResult<IEnumerable<TagUpdateDTO>>> GetLatestTags()
    {
        var tags = await _context.SharkTagData
            .Include(t => t.Shark)
            .OrderByDescending(x => x.Timestamp)
            .Take(20)
            .Select(x => new TagUpdateDTO
            {
                SharkName = x.Shark.Name,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Temperature = x.WaterTemperature,
                Depth = x.Depth,
                Battery = x.Battery,
                Time = x.Timestamp,
                Status = x.Status
            })
            .ToListAsync();

        return Ok(tags);
    }

    /// <summary>
    /// All shark tag locations for full map view
    /// GET /api/dashboard/all-map-data?from=...&to=...
    /// </summary>
    [HttpGet("all-map-data")]
    public async Task<ActionResult<IEnumerable<HeatmapDTO>>> GetAllMapData(DateTime? from = null, DateTime? to = null)
    {
        var fromDt = from ?? DateTime.UtcNow.AddDays(-7);
        var toDt = to ?? DateTime.UtcNow;

        var mapData = await _context.SharkTagData
            .Where(s => s.Timestamp >= fromDt && s.Timestamp <= toDt)
            .Select(s => new HeatmapDTO
            {
                Lat = s.Latitude,
                Lng = s.Longitude,
                Intensity = 1 // كل نقطة لها قيمة واحدة
            })
            .ToListAsync();

        return Ok(mapData);
    }
}
