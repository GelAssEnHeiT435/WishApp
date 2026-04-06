using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Handlers.WishlistHandlers;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class WishlistController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WishlistController> _logger;

        public WishlistController(IMediator mediator, ILogger<WishlistController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{token:guid}")]
        public async Task<ActionResult> Wishlist(
            Guid token,
            CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            GetWishlistByTokenCommand command = new GetWishlistByTokenCommand(token, url);

            try {
                WishlistDto result = await _mediator.Send(command, ct);
                return View("Wishlist", result);
            }
            catch (EntityNotFoundException) {
                return View("NotFound");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
