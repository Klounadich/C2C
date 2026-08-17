namespace Catalog.Models;

public class Items
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string category { get; set; }
    public string title { get; set; }
    public decimal price { get; set; }
    public Guid city { get; set; }
    public string img_link { get; set; }
    public int views { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
}