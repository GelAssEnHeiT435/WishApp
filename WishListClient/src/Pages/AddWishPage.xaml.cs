using WishListClient.src.ViewModels;

namespace WishListClient.src.Pages;

public partial class AddWishPage : ContentPage
{
	public AddWishPage(AddWishViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}