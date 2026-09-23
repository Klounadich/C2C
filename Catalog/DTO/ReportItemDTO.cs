using MediatR;
using Shared.Responce;

namespace Catalog.DTO;

public class ReportItemDTO : IRequest<Result>
{
    public Guid itemId { get; set; }
   public  string text {get;set;}
    public Guid? userId  {get;set;} = null;
}