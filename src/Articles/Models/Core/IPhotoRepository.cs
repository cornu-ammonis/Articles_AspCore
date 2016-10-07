using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models.Core
{
   public interface IPhotoRepository
    {

        long SaveUploadedFiles(IList<IFormFile> files, IHostingEnvironment hostingEnv);
        List<string> BatchFilePaths(int batch, IHostingEnvironment hostingEnv);



    }
}
