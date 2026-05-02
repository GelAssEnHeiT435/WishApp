using Refit;
using WishListClient.src.Interfaces;
using WishListClient.src.Models;

namespace WishListClient.src.Services
{
    public class AuthService: IAuthService
    {
        private readonly IAuthApi _api;
        private readonly ITokenStorage _tokenStorage;

        public AuthService(IAuthApi api, ITokenStorage tokenStorage)
        {
            _api = api;
            _tokenStorage = tokenStorage;
        }

        public async Task UserRegister(string login, string username, string password)
        {
            try
            {
                AuthResponse? response = await _api.Register(new RegisterRequest(login, username, password));

                if (!string.IsNullOrEmpty(response?.jwt))
                    await _tokenStorage.SaveTokenAsync(response?.jwt);
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    $"Регистрация не удалась. Проверьте подключение к интернету!", "ОК");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Неизвестная ошибка",
                  string.Empty, "OK");
            }
        }

        public async Task UserLogin(string login, string password)
        {
            try
            {
                AuthResponse? response = await _api.Login(new LoginRequest(login, password));

                if (!string.IsNullOrEmpty(response?.jwt))
                    await _tokenStorage.SaveTokenAsync(response?.jwt);
            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    $"Авторизация не удалась. Проверьте подключение к интернету!", "ОК");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Неизвестная ошибка",
                  string.Empty, "OK");
            }
        }
    }
}
