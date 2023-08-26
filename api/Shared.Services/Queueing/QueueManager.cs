using System.Text;
using System.Text.Json;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using Shared.Services.Options;

namespace Shared.Services.Queueing;

public interface IQueueManager
{
    Task SendMessageAsync(string queueName, object message, CancellationToken cancellationToken);
    Task SendMessageAsync(
        string queueName, 
        object message, 
        TimeSpan visibilityTimeout, 
        CancellationToken cancellationToken);
}

public class QueueManager : IQueueManager
{
    private readonly AzureStorageOptions _options;

    public QueueManager(IOptions<AzureStorageOptions> options)
    {
        _options = options.Value;
        
        _options.Verify();
    }

    public async Task SendMessageAsync(string queueName, object message, CancellationToken cancellationToken)
    {
        var client = await GetQueueClientAsync(queueName, cancellationToken);
        var base64Message = GetBase64Message(message);

        await client.SendMessageAsync(base64Message, cancellationToken);
    }

    private static string GetBase64Message(object message)
    {
        var jsonMessage = JsonSerializer.Serialize(message);
        var bytesMessage = Encoding.UTF8.GetBytes(jsonMessage);
        var base64Message = Convert.ToBase64String(bytesMessage);
        return base64Message;
    }

    public async Task SendMessageAsync(
        string queueName,
        object message,
        TimeSpan visibilityTimeout,
        CancellationToken cancellationToken)
    {
        var client = await GetQueueClientAsync(queueName, cancellationToken);
        var base64Message = GetBase64Message(message);

        await client.SendMessageAsync(base64Message, visibilityTimeout, null, cancellationToken);
    }

    private async Task<QueueClient> GetQueueClientAsync(string queueName, CancellationToken cancellationToken)
    {
        var client = new QueueClient(_options.ConnectionString, queueName);

        await client.CreateIfNotExistsAsync(null, cancellationToken);

        if (await client.ExistsAsync(cancellationToken))
        {
            return client;
        }

        throw new InvalidOperationException($"Queue {queueName} could not be created on connection {_options.ConnectionString}");
    }
}