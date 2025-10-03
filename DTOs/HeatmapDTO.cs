using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharkTracking.Application.DTOs
{
    public class HeatmapDTO
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Intensity { get; set; }

        //public HeatmapDTO(double lat, double lng, double intensity)
        //{
        //    Lat = lat;
        //    Lng = lng;
        //    Intensity = intensity;
        //}
    }
}

