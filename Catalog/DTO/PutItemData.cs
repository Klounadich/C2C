namespace Catalog.DTO;

public class PutItemData
{
   public Guid UserId { get; set; }
   public Guid ItemId { get; set; } = Guid.NewGuid();
   public string itemName { get; set; }
    public string category  { get; set; }
    public decimal price  { get; set; }
    public Guid city  {get; set;}
    public string? image  { get; set; }
}