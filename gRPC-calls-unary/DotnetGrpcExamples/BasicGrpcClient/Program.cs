using BasicGrpcService;
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

    var reply = client.SendMessage(
                  new ChatRequest
                  {
                      Name = name,
                      Message = message
                  });
    Console.WriteLine($"Reply: {reply.Message}");
}

Console.ReadKey();