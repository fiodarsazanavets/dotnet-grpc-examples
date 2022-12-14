using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client;

Console.WriteLine("What is your name?");
var name = Console.ReadLine();

Console.WriteLine($"Hello {name}. You can start chatting now.");
Console.WriteLine("Type 'exit' to stop at any time.");

using var channel = GrpcChannel.ForAddress("http://localhost:5100");
var client = new Chatbot.ChatbotClient(channel);

while (true)
{
    var message = Console.ReadLine();

    if (message == "exit")
        break;

    Console.WriteLine($"Message: {message}");

    using var call = client.SendMessages();
    var readTask = Task.Run(async () =>
    {
        await foreach (var response in call.ResponseStream.ReadAllAsync())
        {
            Console.WriteLine($"Reply: {response.Message}");
        }
    });
    await call.RequestStream.WriteAsync(new ChatRequest
    {
        Name = name,
        Message = message
    });

    await call.RequestStream.WriteAsync(new ChatRequest
    {
        Name = name,
        Message = $"This message came from {name}."
    });

    await call.RequestStream.CompleteAsync();
    await readTask;
}

Console.ReadKey();