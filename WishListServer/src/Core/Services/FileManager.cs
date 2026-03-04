using Microsoft.Extensions.Options;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Options;

namespace WishListServer.src.Core.Services
{
    public class FileManager: IFileManager
    {
        private readonly IWebHostEnvironment _environment;
        private readonly PathsOptions _pathOptions;

        public FileManager(
            IWebHostEnvironment environment,
            IOptions<PathsOptions> pathOptions)
        {
            _environment = environment;
            _pathOptions = pathOptions.Value;
        }

        public async Task<ImageUploadResult?> SaveImageAsync(IFormFile? file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0 || !IsImage(file))
                return null;

            string directory = _pathOptions.ImageStorage;
            var uploadsDir = Path.Combine(_environment.ContentRootPath, directory);
            Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            await file.CopyToAsync(stream, ct);

            return new ImageUploadResult(fileName, $"/api/images/{fileName}");
        }

        public async Task DeleteImageAsync(string fileName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return;

            string fullPath = Path.Combine(_environment.ContentRootPath, _pathOptions.ImageStorage, fileName);

            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        private bool IsImage(IFormFile file)
        {
            var allowed = new[] { "image/jpeg", "image/png", "image/jpg" };
            return allowed.Contains(file.ContentType);
        }
    }
}
