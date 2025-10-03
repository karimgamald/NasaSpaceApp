using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    public class Shark
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Age { get; set; }
        public double Length { get; set; }
        public double Weight { get; set; }
        public string TagId { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        // Navigation
        public ICollection<SharkTagData>? SharkTagData { get; set; }
        public ICollection<PredictionAlert>? PredictionAlerts { get; set; }
    }

}
