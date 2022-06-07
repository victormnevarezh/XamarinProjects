using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppAzureBlob.Services
{
    //Enumerador para identificar el tipo del blob (texto, imagen)
    public enum AzureContainer
    {
        Image,
        Text
    }

    public class AzureService
    {

        public CloudBlobContainer GetContainerAsync(AzureContainer type)
        {
            //SDK 11
            var account = CloudStorageAccount.Parse(Settings.Constants.StorageConnection);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(type.ToString().ToLower());

            //SDK 12
            /*BlobServiceClient serviceClient = new BlobServiceClient(Settings.Constants.StorageConnection);
            BlobContainerClient containerClient = await serviceClient.CreateBlobContainerAsync(type.ToString());
            return containerClient;*/
        }

        public async Task<IList<string>> GetFilesListAsync(AzureContainer type)
        {
            // Obtiene todos los archivos blob del contenedor especificado en "containerType"

            //SDK 11

            var container = GetContainerAsync(type);
            var list = new List<string>();
            BlobContinuationToken token = null;
            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    list.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);
            return list;

            //SDK 12

            /*var container = await GetContainerAsync(type);
            var list = new List<string>();

            /*await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                list.Add(blobItem.Name);
            }
            return list;*/
        }

        public async Task<byte[]> GetFileAsync(AzureContainer type, string name)
        {
            // Descarga el archivo blob con el nombre "name" del contenedor "containerType"

            //SDK 11
            var container = GetContainerAsync(type);
            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;

            //SDK 12
            /*string localPath = "./temp/";
            string fileName = $"{name}.tmp";
            string filePath = Path.Combine(localPath, fileName);

            BlobContainerClient container = await GetContainerAsync(type);
            BlobClient blob = container.GetBlobClient(name);
            if(await blob.ExistsAsync())
            {
                await blob.DownloadToAsync(filePath); //Response response = 
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] blobBytes = new byte[stream.Length];
                await stream.ReadAsync(blobBytes, 0, (int)stream.Length);
                return blobBytes;
            }
            return null;*/
        }

        public async Task<string> UploadFileAsync(AzureContainer type, Stream stream)
        {
            // Subimos un archivo "stream" al contenedor "containerType"

            //SDK 11
            var container = GetContainerAsync(type);
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);

            return name;

            //SDK 12
            /*var name = Guid.NewGuid().ToString();
            BlobContainerClient container = await GetContainerAsync(type);
            BlobClient blob = container.GetBlobClient(name);
            await blob.UploadAsync(stream, true);
            return name;*/
        }

        public async Task<bool> DeleteFileAsync(AzureContainer type, string name)
        {
            // Borra un archivo blob "name" de un contenedor "containerType"

            //SDK 11

            var container = GetContainerAsync(type);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();

            //SDK 12
            /*BlobContainerClient container = await GetContainerAsync(type);
            BlobClient blob = container.GetBlobClient(name);
            return await blob.DeleteIfExistsAsync();*/
        }

        public async Task<bool> DeleteContainerAsync(AzureContainer type)
        {
            // Borra un contenedor "containerType"

            //SDK 11

            var container = GetContainerAsync(type);
            return await container.DeleteIfExistsAsync();

            //SDK 12
            /*BlobContainerClient container = await GetContainerAsync(type);
            return await container.DeleteIfExistsAsync();*/
        }
    }
}
