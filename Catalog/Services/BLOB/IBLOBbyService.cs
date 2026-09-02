using Microsoft.AspNetCore.Http;

namespace Catalog.Services.BLOB;

public interface IBLOBbyService
{
    public Task<string> UploadFile(IFormFile file , Guid Id);
    
}