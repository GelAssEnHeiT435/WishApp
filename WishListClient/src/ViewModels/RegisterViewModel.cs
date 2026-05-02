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
    public partial class RegisterViewModel: ObservableObject
    {
        private readonly IAuthService _auth;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterExecuteCommand))]
        private string _login = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterExecuteCommand))]
        private string _username = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterExecuteCommand))]
        private string _password = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RegisterExecuteCommand))]
        private string _retryPassword = string.Empty;

        public RegisterViewModel(IAuthService auth)
        {
            _auth = auth;
        }

        [RelayCommand(CanExecute = nameof(canRegister))]
        private async Task RegisterExecute()
        {
            await _auth.UserRegister(Login, Username, Password);

            Login = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            RetryPassword = string.Empty;

            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        [RelayCommand]
        private async Task ToBack() =>
            await Shell.Current.GoToAsync("..");

        private bool canRegister() =>
            !string.IsNullOrWhiteSpace(Login) &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password) &&
            !string.IsNullOrWhiteSpace(RetryPassword) &&
            Password.Equals(RetryPassword);
            
    }
}
