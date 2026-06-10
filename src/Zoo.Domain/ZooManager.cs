namespace Zoo.Domain;

public class ZooManager
{
    public const int MaxCapacity = 50;
    private readonly Dictionary<int, Animal> _animals = new();

    public int AddAnimal(Animal animal)
    {
        if (_animals.ContainsKey(animal.Id))
            throw new DuplicateAnimalException(animal.Id);

        if (_animals.Count >= MaxCapacity)
            throw new ZooCapacityExceededException();

        _animals[animal.Id] = animal;
        return animal.Id;
    }
    public Animal? GetAnimal(int id)
    {
        _animals.TryGetValue(id, out var animal);
        return animal;
    }
    public int TotalAnimals => _animals.Count;

    public int TotalCapacityUsed => _animals.Values.Sum(a => a.Status == HealthStatus.Critical ? 2 : 1);

public decimal CalculateDailyRation(int animalId)
    {
        var animal = GetAnimal(animalId);

        if (animal == null)
            throw new ArgumentException($"Animal with ID {animalId} not found.");

        var baseRation = animal.Category switch
        {
            AnimalCategory.Carnivore => 5.0m,
            AnimalCategory.Herbivore => 10.0m,
            AnimalCategory.Omnivore => 7.0m,
            _ => throw new ArgumentException(
                $"Unknown animal category for ID {animalId}.")
        };

        return animal.Status == HealthStatus.Sick
            ? baseRation * 0.7m
            : baseRation;
    }

    public double CalculateDailyCost() => throw new NotImplementedException();
    public IReadOnlyList<Animal> GetCriticalAnimals() => throw new NotImplementedException();
    public bool RemoveAnimal(int id) => throw new NotImplementedException();
}
