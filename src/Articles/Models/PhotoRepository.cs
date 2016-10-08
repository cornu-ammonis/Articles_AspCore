using Articles.Models.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Articles.Models
{
    public class PhotoRepository : IPhotoRepository 
    {
        public long SaveUploadedFiles(IList<IFormFile> files, IHostingEnvironment hostingEnv )
        {
            long size = 0;
            int counter = 0;
            string uploads_dir = Path.Combine(hostingEnv.WebRootPath, "uploads");

            int subCount = System.IO.Directory.GetDirectories(uploads_dir).Count();

            string n_dir = "uploads\\postbatch" + (subCount + 1).ToString();
            string n_dir_fullpath = Path.Combine(hostingEnv.WebRootPath, n_dir);
            Directory.CreateDirectory(n_dir_fullpath);

            foreach (var file in files)
            {
                counter = counter + 1;
                string fullname = file.FileName;
                int doti = fullname.IndexOf('.');
                string subname = fullname.Substring(doti);

                var filename = "imgr" + counter.ToString() + subname;
                filename = Path.Combine(n_dir_fullpath, filename);
                size += file.Length;

                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }



            }



            return size;
        }

        public List<string> BatchFilePaths(int batch, IHostingEnvironment hostingEnv)
        {
            string img_dir = "uploads\\postbatch" + batch.ToString() + "\\";
            string img_dir_fullpath = Path.Combine(hostingEnv.WebRootPath, img_dir);
            IEnumerable<string> filenames = Directory.EnumerateFiles(img_dir_fullpath);

            List<string> filepaths = new List<string>();

            foreach (string filename in filenames)
            {
                int subi = filename.IndexOf("imgr");
                string subpath = filename.Substring(subi);
                subpath = @"\uploads\postbatch" + batch.ToString() + "\\" + subpath;

                filepaths.Add(subpath);
            }

            return filepaths;
        }

    }
}
