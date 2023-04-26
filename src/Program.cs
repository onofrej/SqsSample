using Amazon.SQS;
using SqsSample;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddAWSService<IAmazonSQS>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();