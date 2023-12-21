using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;



// classe responsavel por conectar com event hub
EventHubProducerClient producerClient = new EventHubProducerClient(
    "Endpoint=sb://caminhoessa.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9Fug1u/gfNkFQ3iT0iR8J1vFF7f1BtSiZ+AEhB8QsGk=",
    "eventhubcaminhoessa");



#region jsonDatas
string jsonData = @"{
'CodCAM': 'CM001',
'Temperatura':2,
'Odometro': 100,
}";

string jsonData2 = @"{
'CodCAM': 'CM002',
'Temperatura':-8,
'Odometro': 102110,
}";

string jsonData3= @"{
'CodCAM': 'CM003',
'Temperatura':-2,
'Odometro': 1000,
}";

string jsonData4 = @"{
'CodCAM': 'CM004',
'Temperatura':35,
'Odometro': 10223,
}";

string jsonData5 = @"{
'CodCAM': 'CM005',
'Temperatura':-5,
'Odometro': 1001234,
}";

string jsonData6 = @"{
'CodCAM': 'CM006',
'Temperatura':28,
'Odometro': 232143,
}";

string jsonData7 = @"{
'CodCAM': 'CM007',
'Temperatura':10,
'Odometro': 104560,
}";

string jsonData8 = @"{
'CodCAM': 'CM008',
'Temperatura':-12,
'Odometro': 1023875,
}";

string jsonData9 = @"{
'CodCAM': 'CM009',
'Temperatura':-9,
'Odometro': 100798,
}";

string jsonData10 = @"{
'CodCAM': 'CM010',
'Temperatura':35,
'Odometro': 19876,
}";

string jsonData11 = @"{
'CodCAM': 'CM011',
'Temperatura':2,
'Odometro': 2138739,
}";

string jsonData12 = @"{
'CodCAM': 'CM012',
'Temperatura':-9 ,
'Odometro': 3459,
}";
#endregion


string[] jsonCompleto = {jsonData, jsonData2, jsonData3, jsonData4, jsonData5, jsonData6, jsonData7, jsonData8, jsonData9, jsonData10, jsonData11, jsonData12 };

//Criando lote de eventos
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();


int numOfEvents = 12;

for (int i = 0; i < numOfEvents; i++)
{
    if (!eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(jsonCompleto[i]))))
    {
        Console.WriteLine(jsonData + i);
         
        throw new Exception($"Event is too large for the batch and cannot be sent.");
    }

}

try
{
    await producerClient.SendAsync(eventBatch);
    Console.WriteLine($" o Lote com {numOfEvents} eventos foi publicado.");
}
finally
{
    await producerClient.DisposeAsync();
}