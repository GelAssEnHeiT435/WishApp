using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class RegenerateLinkCommand(Guid userId, string baseUrl): IRequest<WishlistLinkDto>;

    public class RegenerateLinkHandler : IRequestHandler<RegenerateLinkCommand, WishlistLinkDto>
    {
        private readonly ApplicationContext _context;

        public RegenerateLinkHandler(ApplicationContext context) =>
            _context = context;

        public async Task<WishlistLinkDto> Handle(RegenerateLinkCommand request, CancellationToken ct)
        {
            Wishlist? list = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == request.userId, ct);

            if (list == null)
                throw new EntityNotFoundException();

            list.ShareToken = Guid.NewGuid();
            await _context.SaveChangesAsync(ct);

            return new WishlistLinkDto
            {
                Url = list.ShareToken != null
                    ? $"{request.baseUrl}/wishlist/{list.ShareToken}"
                    : null
            };
        }
    }
}
