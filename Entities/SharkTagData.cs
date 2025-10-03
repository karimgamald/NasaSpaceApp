using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    public class SharkTagData
    {
        public int Id { get; set; }
        public int SharkId { get; set; }
        // وقت القراءة
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Depth { get; set; }
        public double WaterTemperature { get; set; }
        public double Battery { get; set; }
        public string Status { get; set; }  
        public double OxygenLevel { get; set; }
        public double Chlorophyll { get; set; }
        public string MovementPattern { get; set; } = string.Empty;
        public string MediaUrl { get; set; } // رابط الصورة أو الفيديو
        public string MediaType { get; set; } // Image / Video

        // Navigation
        public Shark? Shark { get; set; }
    }
}
