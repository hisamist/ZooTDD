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

    public double CalculateDailyCost()
    {
        double total = 0;

        foreach (var animal in _animals.Values)
        {
            if (animal.Category == AnimalCategory.Carnivore)
                total += 25.0;
            else if (animal.Category == AnimalCategory.Herbivore)
                total += 8.0;
            else if (animal.Category == AnimalCategory.Omnivore)
                total += 15.0;

            if (animal.Status == HealthStatus.Sick)
                total += 20.0;
            else if (animal.Status == HealthStatus.Critical)
                total += 50.0;
        }

        return total;
    }

    public IReadOnlyList<Animal> GetCriticalAnimals()
        => _animals.Values.Where(a => a.Status == HealthStatus.Critical).ToList();
    public bool RemoveAnimal(int id) => _animals.Remove(id);

    public IReadOnlyList<Animal> GetAnimalsByCategory(AnimalCategory category)
    {
        return _animals.Values.Where(a => a.Category == category).ToList();
    }
}
