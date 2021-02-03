using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq.Extensions;

namespace wsw1.Pages.Pipeline.DTO
{
    public class Pipeline
    {
        private List<ModelPoint> _points;
        private string _waypointsToLatLongCsv;
        public string Colour { get; set; } = "f0f";

        public List<ModelPoint> Points
        {
            get => _points;
            set
            {
                _points = value;
                Waypoints = GetWaypoints();
            }
        }

        public List<ModelPoint> ElevationPoints
        {
            get ;
            set;
        }
        public List<ModelPoint> Waypoints { get; private set; }
        //dotnet add package morelinq --version 3.3.2
        public List<ModelPoint> GetWaypoints()
        {
            var value = new List<ModelPoint>();
            value.AddRange(GetWaypointsBetween(Points[0], Points[1]));
            value.AddRange(GetWaypointsBetween(Points[1], Points[2]));
            _waypointsToLatLongCsv = String.Join(",", value.Select(w => w.ToLatLongCsv));
            return value;
        }

        public string WaypointsToLatLongCsv => _waypointsToLatLongCsv;

        List<ModelPoint> GetWaypointsBetween(ModelPoint origin, ModelPoint destination)
        {
            var value = new List<ModelPoint>();
            var numPoints = 10;
            Enumerable.Range(0, numPoints).ForEach(i =>
                value.Add(new ModelPoint()
                {
                    Lat = origin.Lat + i * ((destination.Lat - origin.Lat) / numPoints),
                    Long = origin.Long + i * ((destination.Long - origin.Long) / numPoints),
                    Name = $"{origin.Name}_{i}"
                }));
            return value;
        }
    }
}
