using Amazon.SQS;
using Amazon.SQS.Model;

namespace SqsConsumerSample
{
    public class Worker : BackgroundService
    {
        private readonly IAmazonSQS _amazonSQS;

        public Worker(IAmazonSQS amazonSQS)
        {
            _amazonSQS = amazonSQS;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var queueResponse = await _amazonSQS.GetQueueUrlAsync("SampleSqsQueue", stoppingToken);

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueResponse.QueueUrl,
                MaxNumberOfMessages = 10,
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _amazonSQS.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

                foreach (var message in response.Messages)
                {
                    Console.WriteLine($"Message ID: {message.MessageId}");
                    Console.WriteLine($"Message Body: {message.Body}");

                    _ = await _amazonSQS.DeleteMessageAsync(queueResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                }
            }
        }
    }
}