namespace Catalog.Models;

public class Reports
{
  public  Guid Id { get; set; }= Guid.NewGuid();
  public Guid ItemId { get; set; }
  public  Guid? userId { get; set; }
  public string message { get; set; }
  public DateTime created_at { get; set; } = DateTime.UtcNow;
  public string status { get; set; } = "active";
}