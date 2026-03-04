using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Handlers
{
    public record class DeleteWishCommand(Guid wishId): IRequest;

    public class DeleteWishHandler : IRequestHandler<DeleteWishCommand>
    {
        private readonly ApplicationContext _context;
        private readonly IFileManager _fileManager;

        public DeleteWishHandler(ApplicationContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task Handle(DeleteWishCommand request, CancellationToken ct)
        {
            Wish? wish = await _context.Wishes
                .Include(w => w.Image)
                .FirstOrDefaultAsync(w => w.WishId == request.wishId);

            if (wish == null) throw new EntityNotFoundException(nameof(Wish), request.wishId);
            if (wish.Image != null) await _fileManager.DeleteImageAsync(wish.Image.Name, ct);

            _context.Wishes.Remove(wish);
            await _context.SaveChangesAsync(ct);
        }
    }
}
