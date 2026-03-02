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

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseNpgsql(builder.Configuration["Connections:Postgres"]));

            builder.Services.AddOptions<PathsOptions>()
                .BindConfiguration("Paths")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddSingleton<IFileManager, FileManager>();

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

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
