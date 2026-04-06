namespace WishListServer.src.Core.Exceptions
{
    public class NotMatchPasswordException: Exception
    {
        public NotMatchPasswordException(string message) : base(message) {}
    }
}
