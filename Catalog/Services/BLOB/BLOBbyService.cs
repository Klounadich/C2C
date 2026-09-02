using Microsoft.AspNetCore.Http;

namespace Catalog.Services.BLOB;

public class BLOBbyService : IBLOBbyService
{
    private static readonly HttpClient client = new HttpClient();
    private const string BlobbyBaseUrl = "http://178.236.243.241:7845";
    private const string BucketName = "item-images"; 

    public async Task<string> UploadFile(IFormFile file, Guid id)
    {
        var objectKey = $"{id}.jpg"; 

        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream();
        using var streamContent = new StreamContent(fileStream);

        streamContent.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        content.Add(streamContent, "file", file.FileName);

        var url = $"{BlobbyBaseUrl}/api/blob/bucket/{BucketName}/objects/{objectKey}";

        var response = await client.PutAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"BLOBby upload failed: {response.StatusCode} - {error}");
        }

        return await response.Content.ReadAsStringAsync(); 
    }
}