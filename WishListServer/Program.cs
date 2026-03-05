using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddOptions<PathsOptions>()
                .BindConfiguration("Paths")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddSingleton<IFileManager, FileManager>();

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

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, app.Configuration["Paths:ImageStorage"]!)),
                RequestPath = "/api/images"
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
