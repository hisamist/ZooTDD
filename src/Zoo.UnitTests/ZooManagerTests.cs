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

}
