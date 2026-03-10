namespace WishListServer.src.Core.Exceptions
{
    public class EntityNotFoundException: Exception
    {
        public EntityNotFoundException() :
            base($"Entity not found") { }
        public EntityNotFoundException(string entityName, object id): 
            base($"Entity {entityName} with ID {id} not found") { }
    }
}
