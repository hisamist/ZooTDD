using FluentAssertions;
using Zoo.Domain;

namespace Zoo.UnitTests;

public class ZooManagerTests
{
    [Fact]
    [Trait("Requirement", "REQ-Z-001")]
    public void AddAnimal_ValidAnimal_ReturnsAssignedId()
    {
        // Arrange
        var zoo = new ZooManager();
        var lion = new Animal
        {
            Id = 1,
            Name = "Simba",
            Category = AnimalCategory.Carnivore
        };

        // Act
        var assignedId = zoo.AddAnimal(lion);

        // Assert
        assignedId.Should().Be(1);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-002")]
    public void GetAnimal_ExistingId_ReturnsAnimal()
    {
        // Arrange
        var zoo = new ZooManager();
        var lion = new Animal
        {
            Id = 1,
            Name = "Simba",
            Category = AnimalCategory.Carnivore
        };
        zoo.AddAnimal(lion);

        // Act
        var result = zoo.GetAnimal(1);

        // Assert
        result.Should().BeEquivalentTo(lion);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-003")]
    public void GetAnimal_UnknownId_ReturnsNull()
    {
        // Arrange
        var zoo = new ZooManager();

        // Act
        var result = zoo.GetAnimal(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-004")]
    public void TotalAnimals_AfterAddingThreeAnimals_ReturnsThree()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore });
        zoo.AddAnimal(new Animal { Id = 2, Name = "Zazu", Category = AnimalCategory.Omnivore });
        zoo.AddAnimal(new Animal { Id = 3, Name = "Pumbaa", Category = AnimalCategory.Herbivore });

        // Act
        var total = zoo.TotalAnimals;

        // Assert
        total.Should().Be(3);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-005")]
    public void AddAnimal_DuplicateId_ThrowsDuplicateAnimalException()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore });
        // Act
        Action act = () => zoo.AddAnimal(new Animal
        {
            Id = 1,
            Name = "Nala",
            Category = AnimalCategory.Carnivore
        });

        //Assert
        act.Should().Throw<DuplicateAnimalException>()
        .WithMessage("*1 already exists*");
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-006")]
    public void AddAnimal_AtMaxCapacity_ThrowsZooCapacityExceededException()
    {
        // Arrange
        var zoo = new ZooManager();
        for (var i = 1; i <= ZooManager.MaxCapacity; i++)
        {
            zoo.AddAnimal(new Animal { Id = i, Name = $"Animal{i}", Category = AnimalCategory.Herbivore });
        }

        // Act
        Action act = () => zoo.AddAnimal(new Animal { Id = 51, Name = "Extra", Category = AnimalCategory.Herbivore });

        // Assert
        act.Should().Throw<ZooCapacityExceededException>();
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-007")]
    public void TotalCapacityUsed_OneCriticalAnimal_CountsAsTwo()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Healthy });
        zoo.AddAnimal(new Animal { Id = 2, Name = "Nala", Category = AnimalCategory.Carnivore, Status = HealthStatus.Critical });

        // Act
        var result = zoo.TotalCapacityUsed;

        // Assert
        result.Should().Be(3);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-008")]
    public void CalculateDailyRation_ForACarnivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var lion = new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore };
        zoo.AddAnimal(lion);

        // Act
        var dailyRation = zoo.CalculateDailyRation(lion.Id);

        // Assert
        dailyRation.Should().Be(5); // Assuming carnivores require 5 kg of food per day
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-008")]
    public void CalculateDailyRation_ForAHerbivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var elephant = new Animal { Id = 1, Name = "Dumbo", Category = AnimalCategory.Herbivore };
        zoo.AddAnimal(elephant);

        // Act
        var dailyRation = zoo.CalculateDailyRation(elephant.Id);

        // Assert
        dailyRation.Should().Be(10); // Assuming herbivores require 10 kg of food per day
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-008")]
    public void CalculateDailyRation_ForAnOmnivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var bear = new Animal { Id = 1, Name = "Baloo", Category = AnimalCategory.Omnivore };
        zoo.AddAnimal(bear);

        // Act
        var dailyRation = zoo.CalculateDailyRation(bear.Id);

        // Assert
        dailyRation.Should().Be(7); // Assuming omnivores require 7 kg of food per day
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-008")]
    public void CalculateDailyRation_ForUnknownAnimal_ThrowsArgumentException()
    {
        // Arrange
        var zoo = new ZooManager();

        // Act
        Action act = () => zoo.CalculateDailyRation(999);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Animal with ID 999 not found.");
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-008")]
    public void CalculateDailyRation_ForUnknownCategory_ThrowsArgumentException()
    {
        // Arrange
        var zoo = new ZooManager();
        var unknownAnimal = new Animal { Id = 1, Name = "Mystery", Category = (AnimalCategory)999 };
        zoo.AddAnimal(unknownAnimal);

        // Act
        Action act = () => zoo.CalculateDailyRation(unknownAnimal.Id);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"Unknown animal category for ID {unknownAnimal.Id}.");
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-009")]
    public void CalculateDailyRation_ForASickAnimalCarnivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var sickLion = new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Sick };
        zoo.AddAnimal(sickLion);

        // Act
        var dailyRation = zoo.CalculateDailyRation(sickLion.Id);

        // Assert
        dailyRation.Should().Be(3.5m); // Assuming the ration is -30 % of health status for this example
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-009")]
    public void CalculateDailyRation_ForASickAnimalHerbivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var sickElephant = new Animal { Id = 1, Name = "Dumbo", Category = AnimalCategory.Herbivore, Status = HealthStatus.Sick };
        zoo.AddAnimal(sickElephant);

        // Act
        var dailyRation = zoo.CalculateDailyRation(sickElephant.Id);

        // Assert
        dailyRation.Should().Be(7);  // Assuming the ration is -30 % of health status for this example
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-009")]
    public void CalculateDailyRation_ForASickAnimalOmnivore()
    {
        // Arrange
        var zoo = new ZooManager();
        var sickBear = new Animal { Id = 1, Name = "Baloo", Category = AnimalCategory.Omnivore, Status = HealthStatus.Sick };
        zoo.AddAnimal(sickBear);

        // Act
        var dailyRation = zoo.CalculateDailyRation(sickBear.Id);

        // Assert
        dailyRation.Should().Be(4.9m);  // Assuming the ration is -30 % of health status for this example
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-010")]
    public void CalculateDailyCost_MultipleHealthyAnimals_ReturnsSumOfCategoryCosts()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Healthy });
        zoo.AddAnimal(new Animal { Id = 2, Name = "Pumbaa", Category = AnimalCategory.Herbivore, Status = HealthStatus.Healthy });
        zoo.AddAnimal(new Animal { Id = 3, Name = "Zazu", Category = AnimalCategory.Omnivore, Status = HealthStatus.Healthy });

        // Act
        var totalCost = zoo.CalculateDailyCost();

        // Assert
        totalCost.Should().Be(48); // 25 (Carnivore) + 8 (Herbivore) + 15 (Omnivore)
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-011")]
    public void CalculateDailyCost_OneSickLion_Returns45Euros()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Sick });

        // Act
        var totalCost = zoo.CalculateDailyCost();

        // Assert
        totalCost.Should().Be(45); // 25 (Carnivore) + 20 (frais vétérinaires Sick)
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-012")]
    public void CalculateDailyCost_OneCriticalLion_Returns75Euros()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Critical });

        // Act
        var totalCost = zoo.CalculateDailyCost();

        // Assert
        totalCost.Should().Be(75); // 25 (Carnivore) + 50 (alerte vétérinaire Critical)
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-013")]
    public void GetCriticalAnimals_ThreeCriticalAndTwoHealthy_ReturnsThree()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "A", Category = AnimalCategory.Carnivore, Status = HealthStatus.Critical });
        zoo.AddAnimal(new Animal { Id = 2, Name = "B", Category = AnimalCategory.Herbivore, Status = HealthStatus.Critical });
        zoo.AddAnimal(new Animal { Id = 3, Name = "C", Category = AnimalCategory.Omnivore, Status = HealthStatus.Critical });
        zoo.AddAnimal(new Animal { Id = 4, Name = "D", Category = AnimalCategory.Herbivore, Status = HealthStatus.Healthy });
        zoo.AddAnimal(new Animal { Id = 5, Name = "E", Category = AnimalCategory.Carnivore, Status = HealthStatus.Healthy });

        // Act
        var criticalAnimals = zoo.GetCriticalAnimals();

        // Assert
        criticalAnimals.Should().HaveCount(3);
        criticalAnimals.Should().OnlyContain(a => a.Status == HealthStatus.Critical);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-014")]
    public void RemoveAnimal_ExistingId_ReturnsTrue()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore });

        // Act
        var result = zoo.RemoveAnimal(1);

        // Assert
        result.Should().BeTrue();
        zoo.TotalAnimals.Should().Be(0);
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-015")]
    public void RemoveAnimal_UnknownId_ReturnsFalse()
    {
        // Arrange
        var zoo = new ZooManager();

        // Act
        var result = zoo.RemoveAnimal(999);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-016")]
    public void CalculateMonthlyCost_ZooWithVariousAnimals_ReturnsExpectedMonthlyCost()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Healthy });
        zoo.AddAnimal(new Animal { Id = 2, Name = "Pumbaa", Category = AnimalCategory.Herbivore, Status = HealthStatus.Sick });
        zoo.AddAnimal(new Animal { Id = 3, Name = "Zazu", Category = AnimalCategory.Omnivore, Status = HealthStatus.Critical });

        // Act
        var dailyCost = zoo.CalculateDailyCost();
        var monthlyCost = dailyCost * 30;

        // Assert
        monthlyCost.Should().Be(3540.0); // 118.0 € × 30 days
    }

    [Fact]
    [Trait("Requirement", "REQ-Z-017")]
    public void GetAnimalsByCategory_MultipleAnimals_ReturnsOnlySpecifiedCategory()
    {
        // Arrange
        var zoo = new ZooManager();
        zoo.AddAnimal(new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore });
        zoo.AddAnimal(new Animal { Id = 2, Name = "Pumbaa", Category = AnimalCategory.Herbivore });
        zoo.AddAnimal(new Animal { Id = 3, Name = "Zazu", Category = AnimalCategory.Omnivore });
        zoo.AddAnimal(new Animal { Id = 4, Name = "Dumbo", Category = AnimalCategory.Herbivore });

        // Act
        var herbivores = zoo.GetAnimalsByCategory(AnimalCategory.Herbivore);

        // Assert
        herbivores.Should().HaveCount(2);
        herbivores.Should().OnlyContain(a => a.Category == AnimalCategory.Herbivore);
    }
}
