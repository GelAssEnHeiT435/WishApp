using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class UpdateWishCommand(
        Guid userId, Guid wishId, string? title, string? description, string? link,
        bool? IsReceived, IFormFile? image): IRequest<UpdateWishResult>;

    public class UpdateWishHandler : IRequestHandler<UpdateWishCommand, UpdateWishResult>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public UpdateWishHandler(ApplicationContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<UpdateWishResult> Handle(UpdateWishCommand request, CancellationToken ct)
        {
            Wish? wish = await _context.Wishes
                .Include(w => w.Wishlist)
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => 
                    w.WishId == request.wishId &&
                    w.Wishlist.UserId == request.userId, ct);

            if (wish == null) throw new EntityNotFoundException(nameof(Wish), request.wishId);

            string? newRelativePath = null;
            if (request.image != null && request.image.Length > 0)
            {
                if (wish.Image != null)
                    await _fileManager.DeleteImageAsync(wish.Image.Name!, ct);

                ImageUploadResult? imageResult = await _fileManager.SaveImageAsync(request.image, ct);
                wish.Image.Name = imageResult.Name;
                wish.Image.Url = imageResult.RelativePath;

                newRelativePath = imageResult.RelativePath;
            }

            if (!string.IsNullOrWhiteSpace(request.title) && !string.Equals(wish.Title, request.title, StringComparison.Ordinal))
                wish.Title = request.title;

            if (!string.IsNullOrWhiteSpace(request.description) && !string.Equals(wish.Description, request.description, StringComparison.Ordinal))
                wish.Description = request.description;

            if (!string.IsNullOrWhiteSpace(request.description) && !string.Equals(wish.Description, request.description, StringComparison.Ordinal))
                wish.Link = request.link;

            if (request.IsReceived.HasValue && wish.IsReceived != request.IsReceived.Value)
                wish.IsReceived = request.IsReceived.Value;

            await _context.SaveChangesAsync(ct);
            return new UpdateWishResult(newRelativePath);
        }
    }
}
