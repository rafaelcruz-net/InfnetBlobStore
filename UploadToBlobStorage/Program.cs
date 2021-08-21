using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UploadToBlobStorage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //ADICIONAR PACOTE AZURE.STORAGE.BLOBS!!!!!!!

            Console.WriteLine("Conectando ao Azure Blob Storage");

            //PEGAR STRING DE CONEXAO NO PORTAL!!!!!!
            String azureBlobStorageConnection =
                "DefaultEndpointsProtocol=https;AccountName=infnetstorage;AccountKey=XTmfoq0uQtRbk/wSXxZTDKFe5wdZOqKXQqY7A69SJIZITep6En7LjoJVXugceQOnGfXVZm1N6L5w7w/VHkmA5g==;EndpointSuffix=core.windows.net";

            //Cria um serviço para o client do blob storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(azureBlobStorageConnection);

            //Conecta ao container, já criado no portal
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("images");

            Console.WriteLine("Conectado com sucesso!");

            Console.WriteLine("Criando arquivo randomicamente!");

            string localPath = "./data/";
            string fileName = "blob_storage_file" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            if (!Directory.Exists(localPath))
                Directory.CreateDirectory(localPath);

            File.WriteAllText(localFilePath, "Hello world do blob storage");

            BlobClient client = containerClient.GetBlobClient(fileName);

            client.Upload(localFilePath);

            Console.WriteLine("Upload realizado com sucesso!");

            Console.WriteLine("Listando os arquivos do container!");

            var localPathToSave = "./data/download/";

            if (!Directory.Exists(localPathToSave))
                Directory.CreateDirectory(localPathToSave);

            var blobClient = containerClient.GetBlobClient("https://infnetstorage.blob.core.windows.net/images/" + fileName);

            await blobClient.DownloadToAsync(localPathToSave + fileName);

            //<img src="https://infnetstorage.blob.core.windows.net/images/fileName.jpg">


            //foreach (var blobItem in containerClient.GetBlobs())
            //{
            //    Console.WriteLine($"Arquivo {blobItem.Name} está contido no container, criado em {blobItem.Properties.CreatedOn}");

            //    var blobClient = containerClient.GetBlobClient(blobItem.Name);

            //    await blobClient.DownloadToAsync(localPathToSave + blobItem.Name);

            //    Console.WriteLine($"Arquivo {blobItem.Name} foi salvo no caminho {localPathToSave}!");
            //}

        }
    }
}
