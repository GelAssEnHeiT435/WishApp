using WishListClient.src.ViewModels;

namespace WishListClient.src.Pages;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailsViewModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}