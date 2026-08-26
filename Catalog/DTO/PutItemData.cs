namespace Catalog.DTO;

public class PutItemData
{
   public Guid UserId { get; set; }
   public string itemName { get; set; }
    public string category  { get; set; }
    public decimal price  { get; set; }
    public Guid city  {get; set;}
    public string? image  { get; set; }
}