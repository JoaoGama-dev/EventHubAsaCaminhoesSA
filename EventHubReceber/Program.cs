using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using System.Text;


//Criar um cliente de contêiner de blob que o processador de eventos usará

BlobContainerClient StorageClient = new BlobContainerClient(
    "DefaultEndpointsProtocol=https;AccountName=sastreamcaminhoessa;AccountKey=djRs7NqMY6WPCApd/WXvFJzqKD5XO3GZlOlVjhrGuu7dC7r4mV/KJpr6hcyP+X1+O0AOrKCDs2J0+ASt9Lx6hQ==;EndpointSuffix=core.windows.net", 
    "bronze");

//Criar um cliente do processador de eventos para processar eventos no hub de eventos
var processor = new EventProcessorClient(
    StorageClient,
    EventHubConsumerClient.DefaultConsumerGroupName,
    "Endpoint=sb://caminhoessa.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9Fug1u/gfNkFQ3iT0iR8J1vFF7f1BtSiZ+AEhB8QsGk=",
    "ehmercadogama");


processor.ProcessEventAsync += ProcessEventHandler;
processor.ProcessErrorAsync += ProcessErrorHandler;

await processor.StartProcessingAsync();

await Task.Delay(TimeSpan.FromSeconds(30));

await processor.StopProcessingAsync();

async Task ProcessEventHandler(ProcessEventArgs eventArgs)
{
    Console.WriteLine("\tReceived event: {0}", Encoding.UTF8.GetString(eventArgs.Data.EventBody.ToArray()));

    await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
}

Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
{

    Console.WriteLine($"\t`Partition '{eventArgs.PartitionId}': foi encontrada uma exception.");
    Console.WriteLine(eventArgs.Exception.Message);
    return Task.CompletedTask;
}

