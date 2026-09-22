using Cart.Commands;
using MediatR;
using Shared.Responce;

namespace Cart.DTO;

public class DeleteItemDTO : IRequest<Result>
{
    public Guid itemId {get; set;}
    public Guid userId {get; set;}
    public Guid cartId {get; set;}
}