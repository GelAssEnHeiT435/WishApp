using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Interfaces
{
    public interface IImageConverter
    {
        Task<StreamPart> ImageSourceToStreamPartAsync(ImageSource imageSource, string fileName, string contentType);
    }
}
