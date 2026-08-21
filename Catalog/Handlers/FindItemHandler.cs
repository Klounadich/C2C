using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;

public class FindItemHandler
    : IRequestHandler<FindItemRequestCommand, FoundItemsResponce>
{
    private const string SynonymsLink =
        "https://ruwordnet.ru/ru/search/";

    private readonly SynonymousWordsParser _synonymousWordsParser;
    private readonly ICatalogRepository _catalogRepository;

    public FindItemHandler(
        SynonymousWordsParser synonymousWordsParser,
        ICatalogRepository catalogRepository)
    {
        _synonymousWordsParser = synonymousWordsParser;
        _catalogRepository = catalogRepository;
    }

    public async Task<FoundItemsResponce> Handle(
        FindItemRequestCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedRequest = request.request.Normalize();
        List<string> synonymousWords;

        try
        {
            var url = SynonymsLink + Uri.EscapeDataString(normalizedRequest);
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) }; 
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/151.0.0.0 Safari/537.36");

            var response = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync(cancellationToken);
            synonymousWords = await _synonymousWordsParser.ParseSynonyms(html);
        }
        catch (Exception)
        {
            
            synonymousWords = new List<string> { normalizedRequest };
        }

        var items = await _catalogRepository.GetItemsAsync(synonymousWords);
        return new FoundItemsResponce(items);
    }
}