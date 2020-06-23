using Contoso.Spaces.Populate.Configuration;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Contoso.Spaces.Populate.Storage
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
            string storageConnectionString = connectionStrings.AzureStorage;

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
    }
}