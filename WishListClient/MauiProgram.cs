using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using System.Buffers.Text;
using WishListClient.src.Interfaces;
using WishListClient.src.Pages;
using WishListClient.src.Services;
using WishListClient.src.ViewModels;

namespace WishListClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddRefitClient<IWishListApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://194.67.127.232"));

            builder.Services.AddTransient<AddWishViewModel>();
            builder.Services.AddTransient<AddWishPage>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<DetailsViewModel>();
            builder.Services.AddTransient<DetailsPage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<IImageConverter, ImageConverter>();
            builder.Services.AddSingleton<WishlistService>();

            return builder.Build();
        }
    }
}
