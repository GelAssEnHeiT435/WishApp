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
            string url = "http://10.111.121.61:5073";

            builder.Services.AddRefitClient<IWishListApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(url));

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<AddWishViewModel>();
            builder.Services.AddSingleton<DetailsViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<DetailsPage>();
            builder.Services.AddSingleton<AddWishPage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<IImageConverter, ImageConverter>();
            builder.Services.AddSingleton<WishlistService>();

            return builder.Build();
        }
    }
}
