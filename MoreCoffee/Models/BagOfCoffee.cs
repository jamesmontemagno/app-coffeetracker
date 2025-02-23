using SQLite;

namespace MoreCoffee.Models;

public class BagOfCoffee
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string? Roaster { get; set; }
    public DateTime RoastDate { get; set; }
    public string? Name { get; set; }
    public RoastLevel RoastLevel { get; set; }
    public string? Origin { get; set; }
    public DateTime DateAdded { get; set; } = DateTime.Now;
    public string? TastingNotes { get; set; }
}