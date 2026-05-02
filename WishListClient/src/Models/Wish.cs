using CommunityToolkit.Mvvm.ComponentModel;
namespace WishListClient.src.Models
{
    public partial class Wish: ObservableObject
    {
        public Guid WishId { get; set; }
        [ObservableProperty] private string title;
        [ObservableProperty] private string? description;
        [ObservableProperty] private string? _link;
        [ObservableProperty] private bool isReceived;
        [ObservableProperty] private string? url;

        public ImageSource? Image =>
            !string.IsNullOrEmpty(Url)
                ? ImageSource.FromUri(new Uri(Url))
                : null;

        partial void OnUrlChanged(string? value)
        {
            OnPropertyChanged(nameof(Image));
        }
    }
}
