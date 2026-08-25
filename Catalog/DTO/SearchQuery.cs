namespace Catalog.DTO;

public sealed class SearchQuery
{
    public string Original { get; init; }
    public string Normalized { get; init; }
    public List<string> Tokens { get; init; }
}