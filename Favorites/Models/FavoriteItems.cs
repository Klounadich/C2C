namespace Favorites.Models;

public class FavoriteItems
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    public DateTime added_at { get; set; } = DateTime.UtcNow;
}