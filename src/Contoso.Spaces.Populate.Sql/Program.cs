using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Contoso.Spaces.Populate.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Contoso.Spaces.Populate.Sql
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables();
            IConfigurationRoot configuration = builder.Build();

            ConnectionStrings connectionStrings = configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            string sqlConnectionString = connectionStrings.AzureSqlDb;

            Console.WriteAscii("Seeding SQL Database");
            Console.WriteLine($"Connection String:\t{sqlConnectionString}");

            DbContextOptions<ContosoSpacesContext> options = new DbContextOptionsBuilder<ContosoSpacesContext>()
                .UseSqlServer(sqlConnectionString)
                .Options;

            using ContosoSpacesContext context = new ContosoSpacesContext(options);

            await context.Database.EnsureCreatedAsync();
            Console.WriteLine("Creating Database");

            string seedJsonPath = Path.Combine(Environment.CurrentDirectory, "seed.json");
            string json = await File.ReadAllTextAsync(seedJsonPath);

            List<Location> locations = JsonConvert.DeserializeObject<List<Location>>(json);
                       
            context.Locations.AddRange(locations);

            foreach (var location in locations)
            {
                Console.WriteLine($"Submitting\t{location.Name}");
            }

            await context.SaveChangesAsync();
        }
    }
}