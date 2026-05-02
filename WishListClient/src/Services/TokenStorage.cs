using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;

namespace WishListClient.src.Services
{
    public class TokenStorage: ITokenStorage
    {
        private const string AccessTokenKey = "jwt_access_token";

        public async Task SaveTokenAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                await RemoveTokenAsync();
                return;
            }
            await SecureStorage.SetAsync(AccessTokenKey, token);
        }

        public async Task<string?> GetTokenAsync()
        {
            try {
                return await SecureStorage.GetAsync(AccessTokenKey);
            }
            catch (Exception ex) {
                Debug.WriteLine($"SecureStorage error: {ex.Message}");
                return null;
            }
        }

        public Task RemoveTokenAsync()
        {
            SecureStorage.Remove(AccessTokenKey);
            return Task.CompletedTask;
        }
    }
}
