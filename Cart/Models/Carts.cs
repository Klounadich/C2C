namespace Cart.Models;

public class Carts
{
   public Guid id { get; set; } = Guid.NewGuid();
    public Guid userId { get; set; }
   public  DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public string status { get; set; } = "active";
    
    
}