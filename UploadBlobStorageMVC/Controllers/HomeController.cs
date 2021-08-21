using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UploadBlobStorageMVC.Models;

namespace UploadBlobStorageMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string LastFileUpload; 

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult UploadFile()
        {   
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(IFormFileCollection flInput)
        {

            foreach (var item in this.Request.Form.Files)
            {

                MemoryStream ms = new MemoryStream();

                item.CopyTo(ms);

                ms.Position = 0;
                
                var fileName = "file_" + Guid.NewGuid() + ".png";

                UploadToAzure(fileName, ms);

            }

            //Pega a referencia do ultimo arquivo que subiu
            //HomeController.LastFileUpload = "https://infnetstorage.blob.core.windows.net/images/" + fileName;

            return View();
        }

        private static void UploadToAzure(string fileName, MemoryStream ms)
        {
            //PEGAR STRING DE CONEXAO NO PORTAL!!!!!!   
            String azureBlobStorageConnection =
                "<Sua Conexao String Obtida Pelo Portal Azure>";
            //Cria um serviço para o client do blob storage
            BlobServiceClient blobServiceClient = new BlobServiceClient(azureBlobStorageConnection);

            //Conecta ao container, já criado no portal
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("images");

            //Criar dinamicamente o nome do arquivo


            //Criar a referencia indicando que é um novo arquivo
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            blobClient.Upload(ms);
            
        }

        public IActionResult ShowFile()
        {
            //Exibi o ultimo arquivo que subiu para o Azure
            ViewBag.ImageUrl = HomeController.LastFileUpload;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
