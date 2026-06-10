namespace Zoo.Domain;

public class ZooManager
{
    public const int MaxCapacity = 50;
    private readonly Dictionary<int, Animal> _animals = new();

    public int AddAnimal(Animal animal)
    {
        _animals[animal.Id] = animal;
        return animal.Id;
    }
    public Animal? GetAnimal(int id)
    {
        _animals.TryGetValue(id, out var animal);
        return animal;
    }
    public int TotalAnimals => throw new NotImplementedException();
    public int TotalCapacityUsed => throw new NotImplementedException();
    public double CalculateDailyRation(int animalId) => throw new NotImplementedException();
    public double CalculateDailyCost() => throw new NotImplementedException();
    public IReadOnlyList<Animal> GetCriticalAnimals() => throw new NotImplementedException();
    public bool RemoveAnimal(int id) => throw new NotImplementedException();
}
