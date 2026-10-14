using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Commands;

public record AddItemCommand(
    Guid UserId,
    string itemName,
    string category,
    decimal price,
    Guid city,
    double latitude,
    double longitude,
    IFormFile? image
    ):IRequest<bool>;