using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using Articles.Models.Core;

namespace Articles.Controllers
{
    [RequireHttps]
    public class ImageController : Controller
    {

        private IHostingEnvironment hostingEnv;
        private IPhotoRepository photoRepository;

        public ImageController(IHostingEnvironment env, IPhotoRepository PhotoRepository)
        {
            this.hostingEnv = env;
            this.photoRepository = PhotoRepository;
        }

        public IActionResult UploadFiles()
        {

            return View();
        }

        [HttpPost]
        public IActionResult UploadFiles(IList<IFormFile> files)
        {
            long size = photoRepository.SaveUploadedFiles(files, hostingEnv);
            ViewBag.Message = String.Format("{0} file(s) /  {1}bytes uploaded successfully!", files.Count, size);

            return View();
        }

        public IActionResult ViewImages(int batch = 1) 
        {
            List<string> filepaths = photoRepository.BatchFilePaths(batch, hostingEnv);

            return View(filepaths);
        }

       
    }
}