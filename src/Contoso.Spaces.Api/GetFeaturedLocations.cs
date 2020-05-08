using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Spaces.Api
{
    public static class GetFeaturedLocations
    {
        [FunctionName("getfeaturedlocations")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);

            log.LogInformation(connectionString);

            DbContextOptions<ContosoSpacesContext> options = new DbContextOptionsBuilder<ContosoSpacesContext>()
                .UseSqlServer(connectionString)
                .Options;

            List<Location> recentLocations = new List<Location>();

            using (ContosoSpacesContext context = new ContosoSpacesContext(options))
            {
                List<Location> locations = await context.Locations
                    .OrderByDescending(o => o.LastRenovationDate)
                    .Take(4)
                    .ToListAsync<Location>();
                
                recentLocations.AddRange(locations);
            }

            return new OkObjectResult(recentLocations);
        }
    }
}