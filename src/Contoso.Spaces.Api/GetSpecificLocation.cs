using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Spaces.Api
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
            if(!Int32.TryParse(request.Query["id"], out id))
            {
                return new BadRequestResult();
            }

            log.LogInformation($"{id}");

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);

            log.LogInformation(connectionString);

            DbContextOptions<ContosoSpacesContext> options = new DbContextOptionsBuilder<ContosoSpacesContext>()
                .UseSqlServer(connectionString)
                .Options;

            Location result = null;

            using (ContosoSpacesContext context = new ContosoSpacesContext(options))
            {
                result = await context.Locations
                    .Include(l => l.Rooms)
                    .Where(l => l.Id == id)
                    .SingleOrDefaultAsync();
            }

            return new OkObjectResult(result);
        }
    }
}