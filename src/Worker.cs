using Amazon.SQS;
using Amazon.SQS.Model;

namespace SqsSample
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAmazonSQS _amazonSQS;

        public Worker(ILogger<Worker> logger, IAmazonSQS amazonSQS)
        {
            _logger = logger;
            _amazonSQS = amazonSQS;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueResponse = await _amazonSQS.GetQueueUrlAsync("SampleSqsQueue");

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueResponse.QueueUrl
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _amazonSQS.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

                foreach (var message in response.Messages)
                {
                    Console.WriteLine($"Message ID: {message.MessageId}");
                    Console.WriteLine($"Message Body: {message.Body}");
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}