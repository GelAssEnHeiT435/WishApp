using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Handlers.AuthHandlers;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<JwtResult>> Register(
            [FromBody] RegisterDto request,
            CancellationToken ct)
        {
            try
            {
                RegisterUserCommand command = new RegisterUserCommand(
                    request.Login, request.Username, request.Password
                );

                JwtResult result = await _mediator.Send(command, ct);
                return Ok(result);
            }
            catch (EntityExistsException)
            {
                return BadRequest("A user with this login is already registered. Please choose a different login or log in.");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtResult>> Login(
            [FromBody] LoginDto request,
            CancellationToken ct)
        {
            try
            {
                LoginUserCommand command = new LoginUserCommand(
                    request.Login, request.Password
                );

                JwtResult result = await _mediator.Send(command, ct);
                return Ok(result);
            }
            catch (EntityNotFoundException)
            {
                return NotFound("The user was not found");
            }
            catch (NotMatchPasswordException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
