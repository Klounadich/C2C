using Catalog.DTO;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Services;

public class AliasSearcher
{
    private readonly CatalogDBContext _dbContext;

    public  AliasSearcher(CatalogDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<SearchTerm>> SearchAsync(
        List<string> tokens,
        CancellationToken cancellationToken)
    {
        var aliases = await _dbContext.Aliases
            .Where(x => tokens.Contains(x.Alias))
            .ToDictionaryAsync(
                x => x.Alias,
                x => x.CanonicalTerm,
                cancellationToken);

        var result = new List<SearchTerm>();

        foreach (var token in tokens)
        {
            var alternatives = new List<string> { token };

            if (aliases.TryGetValue(token, out var canonical))
            {
                alternatives.Add(canonical);
            }

            result.Add(new SearchTerm
            {
                Original = token,
                Alternatives = alternatives.Distinct().ToList()
            });
        }

        return result;
    }
}