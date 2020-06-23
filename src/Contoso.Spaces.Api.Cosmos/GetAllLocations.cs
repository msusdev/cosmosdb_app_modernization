using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
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

            List<Location> locations = await container.GetItemLinqQueryable<Location>()
                .OrderByDescending(o => o.LastRenovationDate)
                .ToListAsync();

            return new OkObjectResult(locations);
        }
    }
}