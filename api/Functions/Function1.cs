using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("Function1")]
        public void Run([QueueTrigger("swish-queue-items", Connection = "StorageConnectionString")] string myQueueItem)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
