namespace Zoo.Domain;

public class ZooManager
{
    public const int MaxCapacity = 50;

    // À implémenter en TDD :
    public int AddAnimal(Animal animal) => throw new NotImplementedException();
    public Animal? GetAnimal(int id) => throw new NotImplementedException();
    public int TotalAnimals => throw new NotImplementedException();
    public int TotalCapacityUsed => throw new NotImplementedException();
    public double CalculateDailyRation(int animalId) => throw new NotImplementedException();
    public double CalculateDailyCost() => throw new NotImplementedException();
    public IReadOnlyList<Animal> GetCriticalAnimals() => throw new NotImplementedException();
    public bool RemoveAnimal(int id) => throw new NotImplementedException();
}
