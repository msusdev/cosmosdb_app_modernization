using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Contoso.Spaces.Populate.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Contoso.Spaces.Populate
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
            string csmsConnectionString = connectionStrings.AzureCosmosDb;
            
            Console.WriteAscii("Seeding Cosmos Database");
            Console.WriteLine($"Connection String:\t{csmsConnectionString}");

            using CosmosClient client = new CosmosClient(csmsConnectionString);

            Database database = await client.CreateDatabaseIfNotExistsAsync("ContosoSpaces");
            Console.WriteLine("Creating Database");

            Container container = await database.CreateContainerIfNotExistsAsync("Locations", "/territory", 1000);
            Console.WriteLine("Creating Container");
            
            string seedJsonPath = Path.Combine(Environment.CurrentDirectory, "seed.json");
            string json = await File.ReadAllTextAsync(seedJsonPath);

            var locations = JsonConvert.DeserializeAnonymousType(json, new[] 
            {
                new 
                {
                    id = default(string),
                    name = default(string),
                    longitude = default(double),
                    latitude = default(double),
                    mailingAddress = default(string),
                    territory = default(string),
                    parkingIncluded = default(bool),
                    conferenceRoomsIncluded = default(bool),
                    receptionIncluded = default(bool),
                    publicAccess = default(bool),
                    lastRenovationDate = default(DateTimeOffset),
                    image = default(string),
                    rooms = new[] 
                    {
                        new
                        {
                            description = default(string),
                            monthlyRate = default(double),
                            seats = default(int),
                            privateFacilities = default(bool),
                            phoneIncluded = default(bool),
                            windows = default(bool),
                            corner = default(bool)
                        }
                    }
                }
            });
                       
            foreach (var location in locations)
            {
                await container.CreateItemAsync(location, new PartitionKey(location.territory));
                Console.WriteLine($"Upserting\t{location.name}");
            }
        }
    }
}