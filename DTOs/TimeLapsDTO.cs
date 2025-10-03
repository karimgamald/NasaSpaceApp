using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class TimeLapseDTO
    {
        public DateTime Timestamp { get; set; } // وقت الصورة أو الفيديو
        public string MediaUrl { get; set; } // رابط الصورة أو الفيديو
        public string MediaType { get; set; } // Image / Video
    }

}
