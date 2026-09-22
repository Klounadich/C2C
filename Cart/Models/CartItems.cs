namespace Cart.Models;

public class CartItems
{
  public  Guid id { get; set; } = Guid.NewGuid();
  public  Guid cart_id { get; set; }
   public  Guid item_id { get; set; }
   public  DateTime added_at { get; set; } =  DateTime.UtcNow;
    
}