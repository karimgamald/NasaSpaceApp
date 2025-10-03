using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class InsightDto
    {
        public int SharkId { get; set; }
        public string SharkName { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
        public double SeaTemperature { get; set; }
        public double Chlorophyll { get; set; }
        public string Prediction { get; set; } = string.Empty;
    }
}
