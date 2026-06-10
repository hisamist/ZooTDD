namespace Zoo.Domain;

public class DuplicateAnimalException : Exception
{
    public DuplicateAnimalException(int id)
        : base($"An animal with id {id} already exists.") { }
}

public class ZooCapacityExceededException : Exception
{
    public ZooCapacityExceededException()
        : base("Zoo capacity (50 animals) exceeded.") { }
}
