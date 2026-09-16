using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Catalog.Services.BLOB;

public class BLOBbyService : IBLOBbyService
{
    private static readonly HttpClient client = new HttpClient();
    private const string BlobbyBaseUrl = "http://178.236.243.241:7845";
    private const string BucketName = "item-images"; 
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;
    
    private static readonly Dictionary<string, byte[][]> AllowedSignatures = new()
    {
        ["image/jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
        ["image/png"]  = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } },
        ["image/webp"] = new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } },
    };

    private static async Task<bool> HasValidImageSignature(IFormFile file)
    {
        var allSignatures = AllowedSignatures.Values.SelectMany(s => s);
        var maxLen = allSignatures.Max(s => s.Length);
        var buffer = new byte[maxLen];

        await using var stream = file.OpenReadStream();
        var read = await stream.ReadAsync(buffer, 0, maxLen);

        return allSignatures.Any(sig => 
            read >= sig.Length && buffer.Take(sig.Length).SequenceEqual(sig));
    }

    public async Task<string> UploadFile(IFormFile file, Guid id)
    {
            if (file.Length <= 0 || file.Length > MaxFileSizeBytes)
                throw new ValidationException("Image must be between 1 byte and 5 MB.");
            var objectKey = $"{id}.jpg";
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedContentTypes.Contains(file.ContentType))
                throw new ValidationException("Unsupported content type.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(ext))
                throw new ValidationException("Unsupported file extension.");

            if (!await HasValidImageSignature(file))
                throw new ValidationException("File content does not match a valid image.");

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