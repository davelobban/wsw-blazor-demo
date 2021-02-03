using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using wsw1.Pages.Pipeline.DTO;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElevationController : ControllerBase
    {
        private readonly ILogger<SlowController> _logger;

        public ElevationController(ILogger<SlowController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<double>> Get(string waypointCsvList)
        {
            return await GetElevationsFromWaypointsCsv(waypointCsvList);
        }

        private static async Task<IEnumerable<double>> GetElevationsFromWaypointsCsv(string waypointCsvList)
        {
            using var client = new HttpClient();
            var apiKey = Environment.GetEnvironmentVariable("BingApiKey001System");
            var url = $"http://dev.virtualearth.net/REST/v1/Elevation/List?points={waypointCsvList}&key={apiKey}";
            var resp = await client.GetAsync(url);
            var jsonContent = await resp.Content.ReadAsStringAsync();
            var content = JsonDocument.Parse(jsonContent);
            var elevations = content.RootElement.GetProperty("resourceSets").EnumerateArray().First().GetProperty("resources")
                .EnumerateArray().First().GetProperty("elevations").EnumerateArray().Select(x => x.GetDouble()).ToList();
            ;
            return elevations;
        }

        [HttpPost]
        public async Task<Pipeline> Post([FromBody]Pipeline pipeline)
        {
            var elevations = (await GetElevationsFromWaypointsCsv(pipeline.WaypointsToLatLongCsv)).ToList();
            pipeline.ElevationPoints = new List<ModelPoint>();
            for (var i = 0; i < pipeline.Waypoints.Count; i++)
            {
                pipeline.ElevationPoints.Add(pipeline.Waypoints[i]);
                pipeline.ElevationPoints[i].Elevation = elevations[i];
            }
            return pipeline;
        }
    }
}