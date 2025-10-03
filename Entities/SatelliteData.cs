using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    public class SatelliteData
    {
        public int Id { get; set; }
        public string Source { get; set; } = string.Empty; // e.g., PACE, MODIS, SWOT
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double SeaSurfaceTemperature { get; set; }
        public double ChlorophyllConcentration { get; set; }
        public double TideLevel { get; set; }
        public double LightLevel { get; set; }
        public double OxygenLevel { get; set; }
    }
}
