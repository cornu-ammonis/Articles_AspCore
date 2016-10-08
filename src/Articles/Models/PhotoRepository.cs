
using Articles.Models.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

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

            //creates an uploads\postbatch{#} directory 
            string n_dir = "uploads\\posttbatchh" + (subCount + 1).ToString();
            string n_dir_fullpath = Path.Combine(hostingEnv.WebRootPath, n_dir);
            Directory.CreateDirectory(n_dir_fullpath);

            //creates uploads\postbatch{#}\thumbs directory for thumbnails
            string n_dir_thumbs = n_dir_fullpath + "\\thumbs";
            Directory.CreateDirectory(n_dir_thumbs);

            //creates uploads\postbatch{#}\full directory for full images 
            string n_dir_full = n_dir_fullpath + "\\full";
            Directory.CreateDirectory(n_dir_full);

            foreach (var file in files)
            {
                counter = counter + 1;
                string fullname = file.FileName;
                int doti = fullname.IndexOf('.');
                //gets file extension
                string subname = fullname.Substring(doti);

                var filename = "imgr" + counter.ToString() + subname;
               var fullfilename = Path.Combine(n_dir_full, filename);
                size += file.Length;

                var thumbfilename = Path.Combine(n_dir_thumbs, filename);
                using (FileStream fs2 = System.IO.File.Create(thumbfilename))
                {
                    file.CopyTo(fs2);
                    fs2.Flush();
                    fs2.Dispose();
                }

                using (FileStream fs = System.IO.File.Create(thumbfilename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                    fs.Dispose();
                    fs.Close();
                }

              

               
                int thumbnailsize = 300;
                Bitmap image = new Bitmap(fullfilename);
                int width, height;
                if (image.Width > image.Height)
                {
                    width = thumbnailsize;
                    height = image.Height * thumbnailsize / image.Width;
                }
                else
                {
                    width = image.Width * thumbnailsize / image.Height;
                    height = thumbnailsize;
                }

                /*
                Bitmap target = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                    
                } */
                /*
                Bitmap newImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                // Draws the image in the specified size with quality mode set to HighQuality
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, width, height);
                }

                // Get an ImageCodecInfo object that represents the JPEG codec.
                ImageCodecInfo imageCodecInfo = this.GetEncoderInfo(ImageFormat.Jpeg);

                // Create an Encoder object for the Quality parameter.
                Encoder encoder = Encoder.Quality;

                // Create an EncoderParameters object. 
                EncoderParameters encoderParameters = new EncoderParameters(1);

                // Save the image as a JPEG file with quality level.
                EncoderParameter encoderParameter = new EncoderParameter(encoder, 50);
                encoderParameters.Param[0] = encoderParameter;
                newImage.Save(thumbfilename, imageCodecInfo, encoderParameters);
                */

            }



            return size;
        }

        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
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
