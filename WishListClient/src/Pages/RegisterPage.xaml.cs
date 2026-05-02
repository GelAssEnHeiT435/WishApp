using WishListClient.src.ViewModels;

namespace WishListClient.src.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}