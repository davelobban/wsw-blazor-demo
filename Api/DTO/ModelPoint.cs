using System.Collections.Generic;

namespace wsw1.Pages.Pipeline.DTO
{
    public class ModelPoint
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double[] Coords => new List<double> { Long, Lat }.ToArray();
        public string ToLatLongCsv => $"{Lat},{Long}";
        public double Elevation { get; set; }
    }
}
