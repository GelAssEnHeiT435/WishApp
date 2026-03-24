using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Models;
using WishListClient.src.Pages;
using WishListClient.src.Services;

namespace WishListClient.src.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
        private readonly WishlistService _wishlist;

        public MainViewModel(WishlistService wishlist) =>
            _wishlist = wishlist;

        public ObservableCollection<Wish> Wishes => _wishlist.Wishes;

        [RelayCommand]
        private async Task UpdateList() =>
            await _wishlist.GetAllWishesAsync();

        [RelayCommand]
        private async Task GoToDetails(Wish wish)
        {
            Debug.WriteLine(wish.WishId);
            if (wish != null) 
                await Shell.Current.GoToAsync($"{nameof(DetailsPage)}?id={wish.WishId}");
        }

        [RelayCommand]
        private async Task GoToAddWish() =>
            await Shell.Current.GoToAsync($"{nameof(AddWishPage)}?mode=create");
    }
}
