using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Interfaces
{
    public interface IExceptionHandler
    {
        string? GetMessageException(Exception ex);
    }
}
