using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Interfaces;

namespace WishListClient.src.Services
{
    public class ImageConverter: IImageConverter
    {
        public async Task<StreamPart> ImageSourceToStreamPartAsync(
            ImageSource imageSource,
            string fileName = "image.jpg",
            string contentType = "image/jpeg")
        {
            var bytes = await ImageSourceToBytesAsync(imageSource);
            var stream = new MemoryStream(bytes);

            return new StreamPart(stream, fileName, contentType);
        }

        private async Task<byte[]> ImageSourceToBytesAsync(ImageSource imageSource)
        {
            return imageSource switch
            {
                FileImageSource file => await File.ReadAllBytesAsync(file.File),
                StreamImageSource stream => await ReadStreamAsync(stream.Stream),
                UriImageSource uri => await new HttpClient().GetByteArrayAsync(uri.Uri),
                _ => throw new NotSupportedException("Неподдерживаемый тип ImageSource")
            };
        }

        private async Task<byte[]> ReadStreamAsync(Func<CancellationToken, Task<Stream>> streamFactory)
        {
            using var stream = await streamFactory(CancellationToken.None);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
