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
    public static class ClearCart
    {
        [FunctionName("clearcart")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string user = request.Query["user"];

            if (String.IsNullOrEmpty(user))
            {
                return new BadRequestResult();
            }

            log.LogInformation(user);

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString", EnvironmentVariableTarget.Process);

            log.LogInformation(connectionString);

            DbContextOptions<ContosoSpacesContext> options = new DbContextOptionsBuilder<ContosoSpacesContext>()
                .UseSqlServer(connectionString)
                .Options;

            Cart cart = null;

            using (ContosoSpacesContext context = new ContosoSpacesContext(options))
            {
                cart = await context.Carts
                    .Include(c => c.Locations)
                    .ThenInclude(l => l.Location)
                    .Where(c => c.UserId == user)
                    .SingleOrDefaultAsync();

                if (cart != null)
                {
                    context.Carts.Remove(cart);
                    await context.SaveChangesAsync();
                }
            }

            return new OkResult();
        }
    }
}