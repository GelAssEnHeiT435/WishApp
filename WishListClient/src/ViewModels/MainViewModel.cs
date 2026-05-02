using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Components;
using WishListClient.src.Models;
using WishListClient.src.Pages;
using WishListClient.src.Services;

namespace WishListClient.src.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
        private readonly WishlistService _wishlist;
        private readonly IServiceProvider _provider;
        private bool IsBusy { get; set; }
        private bool _isLoaded;

        public MainViewModel(WishlistService wishlist, IServiceProvider provider)
        {
            _wishlist = wishlist;
            _provider = provider;
        }
            

        public ObservableCollection<Wish> Wishes => _wishlist.Wishes;

        [RelayCommand]
        private async Task LoadWishList()
        {
            if (_isLoaded || IsBusy) return;

            IsBusy = true;
            try {
                await _wishlist.GetAllWishesAsync();
            }
            finally {
                IsBusy = false;
                _isLoaded = true;
            }
        }

        [RelayCommand]
        private async Task OnShare()
        {
            var popup = _provider.GetRequiredService<SharePopup>();
            await (popup.BindingContext as SharePopupViewModel)?.Init()!;

            if (Shell.Current?.CurrentPage is Page page)
                await page.ShowPopupAsync(popup, new PopupOptions
                {
                    CanBeDismissedByTappingOutsideOfPopup = true,
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(20, 20, 20, 20),
                        StrokeThickness = 0,
                        Stroke = Colors.Transparent
                    }
                });
        }
        

        [RelayCommand]
        private async Task GoToDetails(Wish wish)
        {
            if (wish != null) 
                await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={wish.WishId}");
        }

        [RelayCommand]
        private async Task GoToAddWish() =>
            await Shell.Current.GoToAsync($"{nameof(AddWishPage)}?mode=create");
    }
}
