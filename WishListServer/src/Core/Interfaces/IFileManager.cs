using WishListServer.src.Data.Models.Common;

namespace WishListServer.src.Core.Interfaces
{
    public interface IFileManager
    {
        Task<ImageUploadResult?> SaveImageAsync(IFormFile? file, CancellationToken ct);
        Task DeleteImageAsync(string fileName, CancellationToken ct);
    }
}
