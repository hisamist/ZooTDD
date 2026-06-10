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

    public double CalculateDailyRation(int animalId)
    {
        var animal = GetAnimal(animalId);

        if (animal == null)
            throw new ArgumentException($"Animal with ID {animalId} not found.");

        if (animal.Category == AnimalCategory.Carnivore)
            if (animal.Status == HealthStatus.Critical)
                return 3.5; // Example: 5 kg of meat per day reduced by 30% for critical health status
            else
                return 5.0; // Example: 5 kg of meat per day

        if (animal.Category == AnimalCategory.Herbivore)
            return 10.0; // Example: 10 kg of plants per day

        if (animal.Category == AnimalCategory.Omnivore)
            return 7.0; // Example: 7 kg of mixed food per day

        throw new ArgumentException($"Unknown animal category for ID {animalId}.");
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
    public bool RemoveAnimal(int id) => throw new NotImplementedException();
}
