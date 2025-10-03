using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class DashboardSummaryDTO
    {
        public int TotalSharks { get; set; }
        public int ActiveTags { get; set; }
        public double AvgSeaSurfaceTemperature { get; set; }
        public double AvgOxygenLevel { get; set; }
        public IEnumerable<HeatmapDTO> TopHotspots { get; set; } = new List<HeatmapDTO>();
        public IEnumerable<PredictionAlertDTO> RecentAlerts { get; set; } = new List<PredictionAlertDTO>();

        // Empty constructor (needed for serialization)
        public DashboardSummaryDTO() { }

        // Full constructor for quick initialization
        public DashboardSummaryDTO(
            int totalSharks,
            int activeTags,
            double avgSeaSurfaceTemperature,
            double avgOxygenLevel,
            IEnumerable<HeatmapDTO> topHotspots,
            IEnumerable<PredictionAlertDTO> recentAlerts)
        {
            TotalSharks = totalSharks;
            ActiveTags = activeTags;
            AvgSeaSurfaceTemperature = avgSeaSurfaceTemperature;
            AvgOxygenLevel = avgOxygenLevel;
            TopHotspots = topHotspots ?? new List<HeatmapDTO>();
            RecentAlerts = recentAlerts ?? new List<PredictionAlertDTO>();
        }
    }
}
