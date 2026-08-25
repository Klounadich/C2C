namespace Catalog.DTO;

public class SearchTerm
{
    public required string Original { get; init; }

    public List<string> Alternatives { get; init; } = [];
}