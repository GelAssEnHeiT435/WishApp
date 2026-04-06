using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Core.Services;
using WishListServer.src.Data;
using WishListServer.src.Options;

namespace WishListServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
            });

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddOptions<JwtOptions>()
                .BindConfiguration("JwtSettings")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddOptions<PathsOptions>()
                .BindConfiguration("Paths")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            var jwtOptions = builder.Configuration
                .GetSection("JwtSettings")
                .Get<JwtOptions>();
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = AuthService.GetSymmetricSecurityKey(jwtOptions.SecretKey),
                        ValidateIssuerSigningKey = true,
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<IFileManager, FileManager>();

            builder.Services.AddControllersWithViews();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                dbContext.Database.Migrate();
            }

            app.UseForwardedHeaders();

           

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, app.Configuration["Paths:ImageStorage"]!)),
                RequestPath = "/api/images"
            }); // Image's Storage

            app.UseStaticFiles(); // wwwroot

            app.UseRouting();

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}