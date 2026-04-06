using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class GetShareLinkCommand(Guid userId, string baseUrl): IRequest<WishlistLinkDto>;

    public class GetShareLinkHandler : IRequestHandler<GetShareLinkCommand, WishlistLinkDto>
    {
        private readonly ApplicationContext _context;

        public GetShareLinkHandler(ApplicationContext context) =>
            _context = context;

        public async Task<WishlistLinkDto> Handle(GetShareLinkCommand request, CancellationToken ct)
        {
            Guid? shareToken = (await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == request.userId, ct))?.ShareToken;

            if (shareToken == null)
                throw new EntityNotFoundException();

            return new WishlistLinkDto {
                Url = shareToken != null
                    ? $"{request.baseUrl}/wishlist/{shareToken}"
                    : null
            };
        }
    }
}
