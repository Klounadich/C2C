using Catalog.Commands;
using Catalog.DTO;
using Catalog.Repositories;
using MediatR;
using Shared.Responce;

namespace Catalog.Handlers;

public class ReportItemHandler: IRequestHandler<ReportItemDTO,Result>
{
    private readonly ICatalogRepository _repository;

    public ReportItemHandler(ICatalogRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result> Handle(ReportItemDTO request, CancellationToken cancellationToken)
    {
        if (await _repository.AddReportAsync(request))
        {
          return Result.Success();  
        }
        return Result.Failure(errorType: ErrorType.DatabaseError , errorMessage: "Failed to create report");
    }
}