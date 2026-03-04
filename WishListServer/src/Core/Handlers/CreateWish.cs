using MediatR;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Core.Handlers
{
    public record class CreateWishCommand(
        string title,
        string? description,
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

            Wish wish = new Wish
            {
                Title = request!.title,
                Description = request.description,
                IsReceived = request.isRecieved,
                Image = imgObj?.RelativePath != null
                    ? new Image
                    {
                        Name = imgObj?.Name!,
                        Url = imgObj?.RelativePath!
                    } : null
            };

            _context.Wishes.Add(wish);
            await _context.SaveChangesAsync(ct);

            return new CreateWishResult(wish.WishId, imgObj?.RelativePath);
        }
    }
}
