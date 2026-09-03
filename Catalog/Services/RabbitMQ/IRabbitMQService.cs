using Catalog.DTO;

namespace Catalog.Services.RabbitMQ;

public interface IRabbitMQService
{
 
    public Task<bool> SendMessageAsync(ItemModerationDTO message);
}