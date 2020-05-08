using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.Spaces.Api
{
    public static class AddToCart
    {
        [FunctionName("addtocart")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string user = request.Query["user"];

            if (String.IsNullOrEmpty(user))
            {
                return new BadRequestResult();
            }

            log.LogInformation(user);

            string bodyJson = await new StreamReader(request.Body).ReadToEndAsync();

            Location location = JsonConvert.DeserializeObject<Location>(bodyJson);

            log.LogInformation(location.Name);

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

                Location existingLocation = await context.Locations
                    .FindAsync(location.Id);

                if (cart == null)
                {
                    cart = new Cart { UserId = user };
                    context.Carts.Add(cart);
                }
                if (cart.Locations == null)
                {
                    cart.Locations = new List<CartLocation>();
                }
                cart.Locations.Add(
                    new CartLocation { Location = existingLocation }
                );
                await context.SaveChangesAsync();
            }

            IEnumerable<Location> result = cart?.Locations.Select(l => l.Location) ?? Enumerable.Empty<Location>();

            return new OkObjectResult(result);
        }
    }
}