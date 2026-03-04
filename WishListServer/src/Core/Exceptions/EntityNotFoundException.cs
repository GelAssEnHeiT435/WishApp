namespace WishListServer.src.Core.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException(string entityName, object id): 
            base($"Entity {entityName} with ID {id} not found") { }
    }
}
