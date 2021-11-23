using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Data
{
    public static class FileUpload
    {
        public static async Task<string> ImageUploadAsync(IWebHostEnvironment webHostEnvironment, IFormFile formFile)
        {
            string fileName = null, imgFolder, filePath;
            if (formFile.Length > 0)
            {
                fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(formFile.FileName);
                imgFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                filePath = Path.Combine(imgFolder, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
