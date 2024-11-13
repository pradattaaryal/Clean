using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace practices.Helpers
{
    public class ImageHelper
    {
        private readonly string _imageFolderPath;
        public ImageHelper(string imageFolderPath)
        {
            _imageFolderPath = imageFolderPath;
        }
        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid.");
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_imageFolderPath, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }
    }
}
