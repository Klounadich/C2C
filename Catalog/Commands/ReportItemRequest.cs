using StackExchange.Redis;

namespace Catalog.Commands;

public record ReportItemRequest(
    string text,
    Guid itemId
    );
