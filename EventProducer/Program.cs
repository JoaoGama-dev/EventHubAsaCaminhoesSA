using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;



// classe responsavel por conectar com event hub
EventHubProducerClient producerClient = new EventHubProducerClient(
    "Endpoint=sb://caminhoessa.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9Fug1u/gfNkFQ3iT0iR8J1vFF7f1BtSiZ+AEhB8QsGk=",
    "ehmercadogama");

//Criando lote de eventos
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

//numero de eventos para o event Hub
    int numDeEventos = 3;

// testando eventos bem sucedidos 
for (int i = 1; i <= numDeEventos; i++)
{
    if(!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes($"Event {i}"))))
    {
        throw new Exception($"Event {i} grande para o bacth e não pode ser enviado.");
    }
}
//enviar evento com try
try
{
    await producerClient.SendAsync(eventBatch);
    Console.WriteLine($" o Lote com {numDeEventos} eventos foi publicado.");
}
finally
{
    await producerClient.DisposeAsync();
}