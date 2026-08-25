using Catalog.DTO;

public class SearchNormalizer
{
    public SearchQuery Normalize(string query)
    {
        var normalizedQuery = query
            .Trim()
            .ToLowerInvariant();

        var tokens = normalizedQuery
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .ToList();
        Console.WriteLine($"Query: {normalizedQuery}");
        Console.WriteLine($"Tokens: {tokens.Count}");
        return new SearchQuery
        {
            Original = query,
            Normalized = normalizedQuery,
            Tokens = tokens
        };
    }
}