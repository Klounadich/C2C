namespace Catalog.Commands;

public record FindItemRequest(
    string request,
    int page,
    int pageSize);