using WishListClient.src.ViewModels;

namespace WishListClient.src.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(AuthViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}