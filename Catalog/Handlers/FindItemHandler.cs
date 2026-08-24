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
        synonymousWords = await _synonymousWordsParser.GetSynonymousWordsAsync(SynonymsLink,normalizedRequest , cancellationToken);
        var items = await _catalogRepository.GetItemsAsync(synonymousWords);
        return new FoundItemsResponce(items);
    }
}