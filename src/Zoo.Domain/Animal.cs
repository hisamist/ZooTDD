namespace Zoo.Domain;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public AnimalCategory Category { get; set; }
    public HealthStatus Status { get; set; } = HealthStatus.Healthy;
}
