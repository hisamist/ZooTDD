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
}
