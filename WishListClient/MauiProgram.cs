using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using System.Buffers.Text;
using WishListClient.src.Components;
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
                .UseMauiCommunityToolkit(options =>
                {
                    options.SetShouldEnableSnackbarOnWindows(true);
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IClipboard>(Clipboard.Default);
            builder.Services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            builder.Services.AddSingleton<ITokenStorage, TokenStorage>();
            builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.AddRefitClient<IAuthApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://194.67.127.232"));

            builder.Services.AddRefitClient<IWishListApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://194.67.127.232"))
                .AddHttpMessageHandler<AuthHeaderHandler>();

            builder.Services.AddTransient<AuthViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<RegisterPage>();

            builder.Services.AddTransient<AddWishViewModel>();
            builder.Services.AddSingleton<AddWishPage>();

            builder.Services.AddTransient<SharePopup>();
            builder.Services.AddTransient<SharePopupViewModel>();

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddTransient<DetailsViewModel>();
            builder.Services.AddSingleton<DetailsPage>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<IImageConverter, ImageConverter>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<WishlistService>();

            return builder.Build();
        }
    }
}
