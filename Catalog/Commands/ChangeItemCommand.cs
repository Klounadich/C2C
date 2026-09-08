using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Commands;

public record ChangeItemCommand(
    Guid userId,
    Guid ItemId
    ,string itemName,
    string category,
    decimal price,
    Guid city,
    IFormFile? image):IRequest<bool>;