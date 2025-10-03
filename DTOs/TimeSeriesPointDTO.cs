using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class TimeSeriesPointDTO
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Value { get; set; }

        // Empty constructor (needed for serialization)
        public TimeSeriesPointDTO() { }

        // Full constructor
        public TimeSeriesPointDTO(DateTime timestamp, double latitude, double longitude, double value)
        {
            Timestamp = timestamp;
            Latitude = latitude;
            Longitude = longitude;
            Value = value;
        }
    }

}
