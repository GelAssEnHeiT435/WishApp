using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Models;
using WishListClient.src.Pages;
using WishListClient.src.Services;

namespace WishListClient.src.ViewModels
{
    [QueryProperty(nameof(WishId), "id")]
    public partial class DetailsViewModel: ObservableObject
    {
        private readonly WishlistService _wishlist;

        public DetailsViewModel(WishlistService wishlist) =>
            _wishlist = wishlist;

        [ObservableProperty] private ImageSource? _image;

        [ObservableProperty] private Wish _wish;
        public string WishId
        {
            get => _wishId;
            set
            {
                if (value != null)
                {
                    _wishId = value;
                    Wish = _wishlist.GetWishById(Guid.Parse(value))!;

                    if (!string.IsNullOrEmpty(Wish.Url))
                        Image = ImageSource.FromUri(new Uri(Wish?.Url));
                    else
                        Image = null;
                }
            }
        }
        private string _wishId;

        [RelayCommand]
        private async Task GoToEdit()
        {
            if (string.IsNullOrEmpty(WishId)) return;
            await Shell.Current.GoToAsync($"{nameof(AddWishPage)}?mode=edit&id={WishId}");
        }
            

        [RelayCommand]
        private async Task OnDelete()
        {
            await _wishlist.DeleteWish(Wish.WishId);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoToBack() =>
            await Shell.Current.GoToAsync("..");

    }
}
