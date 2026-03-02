using WishListClient.src.ViewModels;

namespace WishListClient.src.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}
