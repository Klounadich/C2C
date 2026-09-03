using System.Text;
using System.Text.Json;
using Catalog.DTO;
using RabbitMQ.Client;

namespace Catalog.Services.RabbitMQ;

public class RabbitMQService : IRabbitMQService
{
    private readonly string _connectionString = "amqp://admin:85914753@178.236.243.241:5672/myapp";

    public async Task<bool> SendMessageAsync(ItemModerationDTO message)
    {
        var factory = new ConnectionFactory { Uri = new Uri(_connectionString) };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: "ItemTitleModeration",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "ItemTitleModeration",
            body: body);

        return true;
    }
}