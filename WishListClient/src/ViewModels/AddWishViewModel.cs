using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;
using WishListClient.src.Models;
using WishListClient.src.Services;

namespace WishListClient.src.ViewModels
{
    [QueryProperty(nameof(Mode), "mode")]
    [QueryProperty(nameof(WishId), "id")]
    public partial class AddWishViewModel: ObservableObject
    {
        private readonly WishlistService _wishlist;
        private readonly IImageConverter _converter;

        [ObservableProperty] private string _pageTitle = "Создание желания";
        [ObservableProperty] private string _buttonTitle = "Добавить";
        

        #region Service Data

        private string _editMode;
        public string Mode { 
            get => _editMode; 
            set {
                if (value.Equals("edit"))
                {
                    PageTitle = "Редактирование желания";
                    ButtonTitle = "Обновить";
                }
                _editMode = value;
            } 
        }

        private string _wishId;
        public string WishId { 
            get => _wishId; 
            set
            {
                if (value != null && Mode.Equals("edit")) {
                    Wish wish = _wishlist.GetWishById(Guid.Parse(value))!;
                    Title = wish.Title;
                    Description = wish.Description;
                    IsReceived = wish.IsReceived;
                    Image = ImageSource.FromUri(new Uri(wish?.Url));
                }
            }
        }
         

        #endregion

        public AddWishViewModel(WishlistService wishlist, IImageConverter converter)
        {
            _wishlist = wishlist;
            _converter = converter;
        }

        #region Entity Data
        [ObservableProperty] private ImageSource? _image;
        private string? _name { get; set; }
        private string? _contentType { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(CreateWishCommand))]
        private string? _title;

        [ObservableProperty] private string? _description;
        [ObservableProperty] private bool _isReceived = false;
        #endregion

        [RelayCommand]
        private async Task PickPhoto()
        {
            try
            {
                if (!MediaPicker.IsCaptureSupported)
                {
                    await Shell.Current.DisplayAlert("Ошибка",
                        "Выбор фото не поддерживается на этом устройстве", "OK");
                    return;
                }

                FileResult? photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Выберите фото желания"
                });

                if (photo == null)
                    return;

                _name = photo.FileName;
                _contentType = photo.ContentType;
                Image = ImageSource.FromFile(photo.FullPath);
            }
            catch (PermissionException ex)
            {
                await Shell.Current.DisplayAlert("Ошибка доступа",
                    $"Необходимы разрешения для доступа к фото.\n{ex.Message}", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    $"Не удалось выбрать фото: {ex.Message}", "OK");
            }
        }

        [RelayCommand(CanExecute = nameof(canCreate))]
        private async Task CreateWish()
        {
            StreamPart? imagePart = null;

            if (Image != null && _name != null) imagePart = await _converter.ImageSourceToStreamPartAsync(Image, _name, _contentType);
                
            if(Mode.Equals("edit"))
                await _wishlist.UpdateWish(Guid.Parse(WishId), Title, Description, IsReceived, imagePart);
            else 
                await _wishlist.CreateWish(Title, Description, IsReceived, imagePart);

            Image = null;
            Title = "";
            Description = "";
            IsReceived = false;

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoBack() =>
            await Shell.Current.GoToAsync("..");

        private bool canCreate() =>
            Title != null && !string.IsNullOrWhiteSpace(Title);
    }
}
