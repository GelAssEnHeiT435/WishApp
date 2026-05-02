using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;
using WishListClient.src.Pages;

namespace WishListClient.src.ViewModels
{
    public partial class AuthViewModel: ObservableObject
    {
        private readonly IAuthService _auth;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginExecuteCommand))]
        private string _login = "popov@gmail.com"; //string.Empty

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginExecuteCommand))]
        private string _password = "123456789"; //string.Empty

        public AuthViewModel(IAuthService auth)
        {
            _auth = auth;
        }

        [RelayCommand(CanExecute = nameof(canLogin))]
        private async Task LoginExecute()
        {
            await _auth.UserLogin(Login, Password);

            Login = string.Empty;
            Password = string.Empty;

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        [RelayCommand]
        private async Task ToRegister() =>
            await Shell.Current.GoToAsync(nameof(RegisterPage));


        private bool canLogin() =>
            !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
    }
}
