namespace WishListServer.src.Core.Exceptions
{
    public class EntityExistsException: Exception
    {
        public EntityExistsException(string entityName, string paramName) :
            base($"Entity {entityName} with parameter {paramName} already exists.")
        { }
    }
}
