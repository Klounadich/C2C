using HtmlAgilityPack;

public class SynonymousWordsParser
{
    public async Task<List<string>> GetSynonymousWordsAsync(string synonymsLink, string normalizedRequest, CancellationToken cancellationToken)
    {
        try
        {
            var url = synonymsLink + Uri.EscapeDataString(normalizedRequest);
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/151.0.0.0 Safari/537.36");

            var response = await client.GetAsync(url, HttpCompletionOption.ResponseContentRead, cancellationToken);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync(cancellationToken);
            return await ParseSynonyms(html);
        }
        catch (Exception)
        {
            return new List<string> { normalizedRequest };
        }
    }
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