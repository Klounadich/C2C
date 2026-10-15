namespace Catalog.Commands;

public record FindItemCategoryRequest(
    string request,
    int page,
    int pageSize);