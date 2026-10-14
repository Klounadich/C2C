using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Catalog.Models;

public class Items
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string category { get; set; }
    public string title { get; set; }
    public decimal price { get; set; }
    public Guid city { get; set; }
    public string? img_link { get; set; } 
    [Column(TypeName = "geography(Point,4326)")]
    public Point? Location { get; set; }
    public int views { get; set; } = 0;
    public bool moderated { get; set; } = false;
    public string status { get; set; } = "sale";
    public DateTime created_at { get; set; }  = DateTime.UtcNow;
    public DateTime updated_at { get; set; }   = DateTime.UtcNow;
}