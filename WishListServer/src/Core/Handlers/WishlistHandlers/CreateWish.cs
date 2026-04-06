using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class CreateWishCommand(
        Guid userId,
        string title,
        string? description,
        string? link,
        bool isRecieved,
        IFormFile? image): IRequest<CreateWishResult>;

    public class CreateWishHandler : IRequestHandler<CreateWishCommand, CreateWishResult>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public CreateWishHandler(
            ApplicationContext context,
            IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }
        public async Task<CreateWishResult> Handle(CreateWishCommand request, CancellationToken ct)
        {
            ImageUploadResult? imgObj = await _fileManager.SaveImageAsync(request?.image, ct);

            Wishlist? list = await _context.Wishlists
                .Include(w => w.Wishes)
                .FirstOrDefaultAsync(w => w.UserId == request!.userId);

            if (list == null)
                throw new EntityNotFoundException();

            Wish wish = new Wish
            {
                Title = request!.title,
                Description = request.description,
                Link = request.link,
                IsReceived = request.isRecieved,
                Image = imgObj?.RelativePath != null
                    ? new Image
                    {
                        Name = imgObj?.Name!,
                        Url = imgObj?.RelativePath!
                    } : null
            };

            list.Wishes!.Add(wish);
            await _context.SaveChangesAsync(ct);

            return new CreateWishResult(wish.WishId, imgObj?.RelativePath);
        }
    }
}
