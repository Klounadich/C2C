using MediatR;
using Shared.Responce;

namespace Cart.DTO;

public class AddItemDTO : IRequest<Result>
{
    public Guid itemId {get; set;}
    public string userId {get; set;}
}