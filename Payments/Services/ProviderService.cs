using System.Net.Http.Json;

namespace Payments.Services;

public class ProviderService
{
    private readonly HttpClient _httpClient;

    public ProviderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendPayment(Guid paymentId,
        decimal amount,
        string currency,
        string description)
    {
        var request = new
        {
            paymentId,
            amount,
            currency,
            description
        };

        var response = await _httpClient.PostAsJsonAsync(
            "http://178.236.243.241:9500/payments",
            request);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadAsStringAsync();

        return result;
    }
}