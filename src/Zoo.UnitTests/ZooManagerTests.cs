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
            zoo.AddAnimal(new Animal { Id = 1, Name = "Simba",Category = AnimalCategory.Carnivore });
            // Act
            Action act = () => zoo.AddAnimal(new Animal { Id = 1, Name = "Nala",
            Category = AnimalCategory.Carnivore });

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
    [Trait("Requirement", "REQ-Z-009")]
    public void CalculateDailyRation_ForASickAnimal()
    {
        // Arrange
        var zoo = new ZooManager();
        var sickLion = new Animal { Id = 1, Name = "Simba", Category = AnimalCategory.Carnivore, Status = HealthStatus.Critical };
        zoo.AddAnimal(sickLion);

        // Act
        var dailyRation = zoo.CalculateDailyRation(sickLion.Id);

        // Assert
        dailyRation.Should().Be(3.5); // Assuming the ration is -30 % of health status for this example
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
}
