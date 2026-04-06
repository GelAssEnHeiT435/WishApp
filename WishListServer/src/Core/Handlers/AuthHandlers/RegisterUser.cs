using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Handlers.AuthHandlers
{
    public record class RegisterUserCommand(string login, string username, string password): IRequest<JwtResult>;
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, JwtResult>
    {
        private readonly ApplicationContext _context;
        private readonly IAuthService _auth;
        public RegisterUserHandler(ApplicationContext context, IAuthService auth)
        { 
            _context = context;
            _auth = auth;
        }

        public async Task<JwtResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.password);

            if (await _context.Credential.AnyAsync(c => c.Login == request.login))
                throw new EntityExistsException(nameof(Credential), request.login);

            User user = new User
            {
                Username = request.username,
                Credential = new Credential
                {
                    Login = request.login,
                    PasswordHash = passwordHash
                },
                Wishlist = new Wishlist
                {
                    ShareToken = Guid.NewGuid()
                }
            };
            _context.Add(user);
            await _context.SaveChangesAsync();

            return new JwtResult(_auth.GenerateJwt(user));
        }
    }
}
