using MediatR;
using Shared.Responce;

namespace Favorites.DTOs;

public class DeleteItemDTO : IRequest<Result>
{
   public Guid itemId { get; set; }
   public Guid userId { get; set; }
}