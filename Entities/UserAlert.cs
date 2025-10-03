using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Core.Entities
{
    // Junction Table for Many-to-Many (Users <-> Alerts)
    public class UserAlert
    {
        public int UserId { get; set; }
        public int PredictionAlertId { get; set; }

        // Navigation
        public User? User { get; set; }
        public PredictionAlert? PredictionAlert { get; set; }
    }
}
