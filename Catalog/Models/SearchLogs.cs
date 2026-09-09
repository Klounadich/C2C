namespace Catalog.Models;

public class SearchLogs
{
    public Guid Id { get; set; } =  Guid.NewGuid();

    public string Query { get; set; }

    public int ResultCount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid? UserId { get; set; }
    public bool processed { get; set; } = false;

    

}