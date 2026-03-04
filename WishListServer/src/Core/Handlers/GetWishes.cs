using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Dto;
using WishListServer.src.Data.Specifications;

namespace WishListServer.src.Core.Handlers
{
    public record class GetWishesCommand(string baseUrl): IRequest<IReadOnlyCollection<WishDto>>;
    public class GetWishesHandler : IRequestHandler<GetWishesCommand, IReadOnlyCollection<WishDto>>
    {
        private readonly ApplicationContext _context;

        public GetWishesHandler(ApplicationContext context) =>
            _context = context;

        public async Task<IReadOnlyCollection<WishDto>> Handle(GetWishesCommand request, CancellationToken cancellationToken) =>
            _context.Wishes
                .AsNoTracking()
                .ToDto(request.baseUrl)
                .ToList();
    }
}
