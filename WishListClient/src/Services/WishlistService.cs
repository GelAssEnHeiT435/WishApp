using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Models;

namespace WishListClient.src.Services
{
    public class WishlistService
    {
        private readonly IWishListApi _api;
        public ObservableCollection<Wish> Wishes { get; } = new();

        public WishlistService(IWishListApi api)
        {
            _api = api;
        }

        public async Task GetAllWishesAsync()
        {
            Wishes.Clear();
            IReadOnlyCollection<Wish> list = await _api.GetWishes();

            foreach (Wish wish in list)
            {
                Wishes.Add(wish);
            }
        }

        public Wish? GetWishById(Guid id) => 
            Wishes.FirstOrDefault(w => w.WishId == id);

        public async Task CreateWish(
            string title, 
            string? description, 
            bool isReceived,
            StreamPart? image)
        {
            CreateWishResponse? result = await _api.CreateWish(title, description, isReceived, image);
            
            if(result != null)
            {
                Wish wish = new Wish()
                {
                    WishId = result.WishId,
                    Title = title,
                    Description = description,
                    IsReceived = isReceived,
                    Url = result.Path
                };
                Wishes.Add(wish);
            }
        }

        public async Task UpdateWish(
            Guid Id,
            string title,
            string? description,
            bool isReceived,
            StreamPart? image)
        {
            try
            {
                UpdateWishResponse response = await _api.UpdateWish(Id, title, description, isReceived, image);
                Wish? wish = Wishes.FirstOrDefault(w => w.WishId == Id);

                if (wish != null)
                {
                    wish.Title = title;
                    wish.Description = description;
                    wish.IsReceived = isReceived;
                    wish.Url = response?.Path ?? null;
                }

            }
            catch (ApiException ex)
            {
                await Shell.Current.DisplayAlert("Ошибка",
                    $"Не удалось обновить желание: {title}", "ОК");
            }
        }

        public async Task DeleteWish(Guid Id)
        {
            try
            {
                await _api.DeleteWish(Id);
                Wishes.Remove(Wishes.FirstOrDefault(w => w.WishId == Id)!);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound) await Shell.Current.DisplayAlert("Ошибка", "Объект не найден!", "Ок");
                await Shell.Current.DisplayAlert("Ошибка", "Проверьте соединение...", "Ок");
            }
        }
    }
}
