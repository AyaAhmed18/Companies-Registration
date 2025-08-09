using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class FileUploadService:IFileUploadService
    {
        private readonly string _rootPath;
        private const long MaxFileSizeInMB = 100;
        private readonly string[] _allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public FileUploadService(IConfiguration configuration)
        {
            _rootPath = configuration["FileStorage:RootPath"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }


        private string NormalizePath(string folderPath)
        {
            // Remove any leading or trailing slashes and replace backward slashes
            folderPath = folderPath.Trim('/').Trim('\\').Replace('\\', '/');

            // Ensure the path doesn't try to navigate up directories
            if (folderPath.Contains(".."))
                throw new ArgumentException("Invalid folder path");

            return folderPath;
        }
        public long GetFileSizeInMegabytes(IFormFile file)
        {
            return file.Length / 1024 / 1024;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("Invalid file");

                if (GetFileSizeInMegabytes(file) > MaxFileSizeInMB)
                    throw new ArgumentException($"File size exceeds maximum limit of {MaxFileSizeInMB}MB");

                // Validate file type
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                var isImage = _allowedImageExtensions.Contains(fileExtension);
                

                if (!isImage)
                    throw new ArgumentException("Invalid file type");

                // Normalize and validate the folder path
                folderPath = NormalizePath(folderPath);

                // Generate unique file name
                var fileName = $"{Guid.NewGuid()}{fileExtension}";

                // Combine paths and create full directory path
                var directoryPath = Path.Combine(_rootPath, folderPath);
                var filePath = Path.Combine(directoryPath, fileName);

              

                // Create file stream and copy file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for storage in database
                return Path.Combine(folderPath, fileName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload file: {ex.Message}", ex);
            }
        }


    }
}
