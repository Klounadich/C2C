using HtmlAgilityPack;

public class SynonymousWordsParser
{
    public async Task<List<string>> ParseSynonyms(string page)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(page);

        var result = htmlDoc
                         .DocumentNode
                         .SelectNodes("//div[contains(@class, 'sense-list')]//div[contains(@class, 'sense')]")
                         ?.Select(x => x.InnerText.Trim())
                         .Where(x => !string.IsNullOrWhiteSpace(x))
                         .Distinct()
                         .ToList()
                     ?? new List<string>();

        return result;
    }
}