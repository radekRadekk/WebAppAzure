namespace WebAppAzure.Repositories;

public class Player
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Country { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public int Height { get; set; }
    public double Weight { get; set; }
    public int CoachId { get; set; }
}