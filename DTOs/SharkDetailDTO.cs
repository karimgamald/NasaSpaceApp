using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class SharkDetailDTO
    {
        public int SharkId { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeLapseDTO TimeLapseDTO { get; set; }
        public IEnumerable<TimeSeriesPointDTO> TagTimeSeries { get; set; } = new List<TimeSeriesPointDTO>();
        public IEnumerable<PredictionAlertDTO> Alerts { get; set; } = new List<PredictionAlertDTO>();

        // Empty constructor (needed for serialization)
        public SharkDetailDTO() { }

        // Full constructor
        public SharkDetailDTO(
            int sharkId,
            string name,
            IEnumerable<TimeSeriesPointDTO> tagTimeSeries,
            IEnumerable<PredictionAlertDTO> alerts)
        {
            SharkId = sharkId;
            Name = name;
            TagTimeSeries = tagTimeSeries ?? new List<TimeSeriesPointDTO>();
            Alerts = alerts ?? new List<PredictionAlertDTO>();
        }
    }

}
