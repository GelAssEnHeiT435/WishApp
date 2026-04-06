using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Dto;
using WishListServer.src.Data.Specifications;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class GetWishlistByTokenCommand(Guid token, string baseUrl): IRequest<WishlistDto>;
    public class GetWishlistByTokenHandler : IRequestHandler<GetWishlistByTokenCommand, WishlistDto>
    {
        private readonly ApplicationContext _context;

        public GetWishlistByTokenHandler(ApplicationContext context) =>
            _context = context;

        public async Task<WishlistDto> Handle(GetWishlistByTokenCommand request, CancellationToken cancellationToken)
        {
            WishlistDto? list = await _context.Wishlists
                .AsNoTrackingWithIdentityResolution()
                .Where(w => w.ShareToken == request.token)
                .ToDto(request.baseUrl)
                .FirstOrDefaultAsync();

            if (list == null) throw new EntityNotFoundException();
            return list;
        }
    }
}
