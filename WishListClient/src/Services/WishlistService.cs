using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    }
}
