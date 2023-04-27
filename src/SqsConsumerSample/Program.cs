using Amazon.SQS;
using SqsConsumerSample;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddAWSService<IAmazonSQS>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();