using System.Text.Json;
using Serilog;
using StackExchange.Redis;

namespace Services.Messaging;

public interface IMessageManager
{
    Task PublishAsync(string channel, object message);
}

public class MessageManager(IConnectionMultiplexer connectionMultiplexer) : IMessageManager
{
    public async Task PublishAsync(string channel, object message)
    {
        var log = Log.ForContext<MessageManager>();
        
        log.Information("Publishing message to {Channel}", channel);
        
        var messageJson = JsonSerializer.Serialize(message);
        var subscriber = connectionMultiplexer.GetSubscriber();

        await subscriber.PublishAsync(channel, messageJson);
    }
}