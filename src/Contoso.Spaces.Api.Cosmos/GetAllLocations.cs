using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Spaces.Api.Solution
{
    public static class GetAllLocations
    {
        [FunctionName("getalllocations")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = Environment.GetEnvironmentVariable("CosmosConnectionString", EnvironmentVariableTarget.Process);

            log.LogInformation(connectionString);

            List<Location> recentLocations = new List<Location>();

            using CosmosClient client = new CosmosClient(connectionString);
            Database database = client.GetDatabase("ContosoSpaces");
            Container container = database.GetContainer("Locations");

            List<Location> locations = new List<Location>();

            string sql = $"SELECT * FROM locations l ORDER BY l.lastRenovationDate DESC";
            var feed = container.GetItemQueryIterator<Location>(sql);

            while (feed.HasMoreResults)
            {
                var results = await feed.ReadNextAsync();
                foreach (var result in results)
                {
                    locations.Add(result);
                }
            }

            return new OkObjectResult(locations);
        }
    }
}