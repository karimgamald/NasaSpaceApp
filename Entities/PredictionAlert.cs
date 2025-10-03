using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    public class PredictionAlert
    {
        public int Id { get; set; }
        public int? SharkId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PredictionType { get; set; } = string.Empty; // e.g., Feeding Zone, Migration
        public string AlertMessage { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty; // Low, Medium, High

        // Navigation
        public Shark? Shark { get; set; }
        public ICollection<UserAlert>? UserAlerts { get; set; }
    }
}
