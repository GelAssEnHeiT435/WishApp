using CommunityToolkit.Mvvm.ComponentModel;
using Java.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Models
{
    public partial class Wish: ObservableObject
    {
        public Guid WishId { get; set; }
        [ObservableProperty] private string title;
        [ObservableProperty] private string? description;
        [ObservableProperty] private bool isReceived;
        [ObservableProperty] private string? url;

        // 🔥 Вычисляемое свойство — не хранится в памяти
        public ImageSource? Image =>
            !string.IsNullOrEmpty(Url)
                ? ImageSource.FromUri(new Uri(Url))
                : null;

        // 🔥 Этот метод вызывается АВТОМАТИЧЕСКИ при изменении Url
        partial void OnUrlChanged(string? value)
        {
            // Говорим UI, что Image тоже изменилось
            OnPropertyChanged(nameof(Image));
        }
    }
}
