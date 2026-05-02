using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using WishListClient.src.Services;

namespace WishListClient.src.ViewModels
{
    public partial class SharePopupViewModel: ObservableObject
    {
        private readonly WishlistService _wishlist;
        private readonly IClipboard _clipboard;

        [ObservableProperty] private string _shareLink;
        [ObservableProperty] private ImageSource _qrCodeImageSource;

        public SharePopupViewModel(WishlistService wishlist, IClipboard clipboard)
        {
            _wishlist = wishlist;
            _clipboard = clipboard;
        }

        public async Task Init() {
            ShareLink = await _wishlist.GetLink() ?? "";
            QrCodeImageSource = GenerateQrCodeImageSource(ShareLink);
        }

        [RelayCommand]
        private async Task CopyLink()
        {
            await Clipboard.SetTextAsync(ShareLink);
            await Toast.Make("Скопировано", ToastDuration.Short, 14.0).Show();
        }

        [RelayCommand]
        private async Task GenerateNewLink() {
            ShareLink = await _wishlist.RegenerateLink() ?? "";
            QrCodeImageSource = GenerateQrCodeImageSource(ShareLink);
        }

        private ImageSource GenerateQrCodeImageSource(string link, int pixelsPerModule = 20)
        {
            if (string.IsNullOrWhiteSpace(link))
                return null;

            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

                using var pngQrCode = new PngByteQRCode(qrCodeData);
                byte[] qrBytes = pngQrCode.GetGraphic(pixelsPerModule);

                return ImageSource.FromStream(() => new MemoryStream(qrBytes));
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Ошибка", "Произошло непредвиденная ошибка при генерации QR-кода", "OK");
                return null;
            }
        }
    }
}
