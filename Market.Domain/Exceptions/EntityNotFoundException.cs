namespace Market.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(Type type)
            : base($"{type.Name} - not found.")
        { }
    }
}
