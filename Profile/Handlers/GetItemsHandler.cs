using System.Security.Claims;
using Catalog.DTO;
using MediatR;
using Profile.Commands;
using Profile.Repositories;


namespace Profile.Handlers;

public class GetItemsHandler: IRequestHandler<GetItemsCommand,FoundItemsResponce>
{
    private readonly IProfileRepository _catalogRepository;

    public GetItemsHandler(IProfileRepository catalogRepository)
    {
        _catalogRepository = catalogRepository;
    }
    public async Task<FoundItemsResponce> Handle(
        GetItemsCommand request, CancellationToken cancellationToken)
    {
       return new FoundItemsResponce(await _catalogRepository.GetItemsAsync(request.UserId, request.page, request.pageSize));
    }
}