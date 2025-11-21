using AutoAzureMob.Models.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Core.AzureBlobServices
{
    public class AzureFileUploader
    {
        public static string connectionString = string.Empty;
        public static string containerName = string.Empty;
        private readonly BlobServiceClient blobServiceClient;

        public AzureFileUploader(IConfiguration config)
        {
            
            blobServiceClient = new BlobServiceClient(connectionString);
            
        }
        public static string UploadFileToBlob(AzureUpload azureUpload)
        {
            string url = string.Empty;
            try
            {
                var blobClient = new BlobContainerClient(azureUpload.ConnectionString, azureUpload.Container);
                using (MemoryStream stream = new MemoryStream())
                {
                    azureUpload.File.CopyTo(stream);
                    var extension = Path.GetExtension(azureUpload.File.Name);
                    stream.Position = 0;
                    var blob = blobClient.GetBlobClient(azureUpload.Folder + "_" + azureUpload.File.Name);
                    blob.UploadAsync(stream).Wait();
                    url = blob.Uri.AbsoluteUri.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e);
               
            }
            return url;
        }
    }
}
