using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class AlertDTO
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Message { get; set; }
    }
}
