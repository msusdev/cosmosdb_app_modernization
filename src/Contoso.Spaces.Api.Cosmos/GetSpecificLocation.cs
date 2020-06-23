using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Spaces.Api.Solution
{
    public static class GetSpecificLocation
    {
        [FunctionName("getspecificlocation")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            int id;
            if (!Int32.TryParse(request.Query["id"], out id))
            {
                return new BadRequestResult();
            }

            log.LogInformation($"{id}");

            string connectionString = Environment.GetEnvironmentVariable("CosmosConnectionString", EnvironmentVariableTarget.Process);

            log.LogInformation(connectionString);

            using CosmosClient client = new CosmosClient(connectionString);
            Database database = client.GetDatabase("ContosoSpaces");
            Container container = database.GetContainer("Locations");

            List<Location> locations = new List<Location>();

            string sql = $"SELECT TOP 1 * FROM locations l WHERE l.id = '{id}'";
            var feed = container.GetItemQueryIterator<Location>();

            while (feed.HasMoreResults)
            {
                var results = await feed.ReadNextAsync();
                foreach (var result in results)
                {
                    locations.Add(result);
                }
            }

            var location = locations.FirstOrDefault();

            if (location is null) { return new NotFoundResult(); }
            else { return new OkObjectResult(location); }
        }
    }
}