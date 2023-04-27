using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

var amazonSQSClient = new AmazonSQSClient(RegionEndpoint.USEast1);

var queueResponse = await amazonSQSClient.GetQueueUrlAsync("SampleSqsQueue");

var tasks = new List<Task>();

for (int counter = 0; counter < 200; counter++)
{
    var sendMessageRequest = new SendMessageRequest
    {
        QueueUrl = queueResponse.QueueUrl,
        MessageBody = $"nome: {counter}"
    };

    tasks.Add(amazonSQSClient.SendMessageAsync(sendMessageRequest));

    Console.WriteLine(counter);
}

await Task.WhenAll(tasks);