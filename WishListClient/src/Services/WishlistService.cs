using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;
using WishListClient.src.Models;

namespace WishListClient.src.Services
{
    public class WishlistService
    {
        private readonly IWishListApi _api;
        private readonly IExceptionHandler _exHandler;
        public ObservableCollection<Wish> Wishes { get; } = new();

        public WishlistService(IWishListApi api, IExceptionHandler exHandler)
        {
            _api = api;
            _exHandler = exHandler;
        }

        public async Task GetAllWishesAsync()
        {
            try 
            {
                Wishes.Clear();
                IReadOnlyCollection<Wish> list = await _api.GetWishes();

                foreach (Wish wish in list)
                {
                    Wishes.Add(wish);
                }
            }
            catch (Exception ex) 
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
            }
        }

        public Wish? GetWishById(Guid id) => 
            Wishes.FirstOrDefault(w => w.WishId == id);

        public async Task CreateWish(
            string title, 
            string? description, 
            string? link,
            bool isReceived,
            ByteArrayPart? image)
        {
            try
            {
                CreateWishResponse? result = await _api.CreateWish(title, description, link, isReceived, image);

                if (result != null)
                {
                    Wish wish = new Wish()
                    {
                        WishId = result.WishId,
                        Title = title,
                        Description = description,
                        Link = link,
                        IsReceived = isReceived,
                        Url = result.Path
                    };
                    Wishes.Add(wish);
                }
            }
            catch (Exception ex)
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
            }
        }

        public async Task UpdateWish(
            Guid Id,
            string title,
            string? description,
            string? link,
            bool isReceived,
            ByteArrayPart? image)
        {
            try
            {
                UpdateWishResponse response = await _api.UpdateWish(Id, title, description, link, isReceived, image);
                Wish? wish = Wishes.FirstOrDefault(w => w.WishId == Id);

                if (wish != null)
                {
                    wish.Title = title;
                    wish.Description = description;
                    wish.Link = link;
                    wish.IsReceived = isReceived;
                    wish.Url = response?.Path ?? null;
                }
            }
            catch (Exception ex)
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
            }
        }

        public async Task DeleteWish(Guid Id)
        {
            try
            {
                await _api.DeleteWish(Id);
                Wishes.Remove(Wishes.FirstOrDefault(w => w.WishId == Id)!);
            }
            catch (Exception ex)
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
            }
        }

        public async Task<string?> GetLink()
        {
            try {
                ShareResponse result = await _api.GetShareLink();
                return result?.url ?? string.Empty;
            }
            catch (Exception ex)
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
                return null;
            }
        }
            

        public async Task<string?> RegenerateLink()
        {
            try {
                ShareResponse result = await _api.RegenerateShareLink();
                return result?.url ?? string.Empty;
            }
            catch (Exception ex)
            {
                string? message = _exHandler.GetMessageException(ex);

                if (!string.IsNullOrEmpty(message))
                    await Shell.Current.DisplayAlert("Ошибка", message, "OK");
                return null;
            }
        }
            
    }
}
