using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class PredictionAlertDTO
    {
        public int Id { get; set; }
        public int? SharkId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PredictionType { get; set; } = string.Empty;
        public string AlertMessage { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;

        // Optional constructor if you want to initialize quickly
        public PredictionAlertDTO(
            int id,
            int? sharkId,
            DateTime timestamp,
            double latitude,
            double longitude,
            string predictionType,
            string alertMessage,
            string riskLevel)
        {
            Id = id;
            SharkId = sharkId;
            Timestamp = timestamp;
            Latitude = latitude;
            Longitude = longitude;
            PredictionType = predictionType;
            AlertMessage = alertMessage;
            RiskLevel = riskLevel;
        }

        // Empty constructor (required for serialization/deserialization)
        public PredictionAlertDTO() { }
    }
}
