using Contoso.Spaces.Data;
using Contoso.Spaces.Models;
using Contoso.Spaces.Populate.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
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

            await UploadStorageBlobs(connectionStrings.AzureStorage);
            
            await SeedSqlDatabase(connectionStrings.AzureSqlDb);
        }

        static async Task UploadStorageBlobs(string storageConnectionString)
        {
            Console.WriteAscii("Uploading Storage Blobs");
            Console.WriteLine($"Connection String:\t{storageConnectionString}");

            CloudStorageAccount account = CloudStorageAccount.Parse(storageConnectionString);

            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("images");
            await container.CreateIfNotExistsAsync();
            Console.WriteLine("Creating Container");

            BlobContainerPermissions permissions = await container.GetPermissionsAsync();
            permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            await container.SetPermissionsAsync(permissions);

            string imagesPath = Path.Combine(Environment.CurrentDirectory, "Images");
            foreach(string file in Directory.EnumerateFiles(imagesPath, "*.png"))
            {
                string filename = Path.GetFileName(file);
                Console.WriteLine($"Uploading\t{filename}");
                CloudBlockBlob blob = container.GetBlockBlobReference(filename);
                await blob.UploadFromFileAsync(file);
            }
        }

        static async Task SeedSqlDatabase(string sqlConnectionString)
        {
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