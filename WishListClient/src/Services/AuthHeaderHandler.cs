using System.Net;
using System.Net.Http.Headers;
using WishListClient.src.Interfaces;
using WishListClient.src.Pages;

namespace WishListClient.src.Services
{
    public class AuthHeaderHandler: DelegatingHandler
    {
        private readonly ITokenStorage _tokenStorage;

        public AuthHeaderHandler(ITokenStorage tokenStorage) =>
            _tokenStorage = tokenStorage;


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenStorage.GetTokenAsync();

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenStorage.RemoveTokenAsync();

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Сессия истекла. Войдите снова.", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                });
            }

            return response;
        }
    }
}
