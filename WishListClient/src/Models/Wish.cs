using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Models
{
    public class Wish
    {
        public Guid WishId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsReceived { get; set; }
        public string? Url { get; set; }
    }
}
