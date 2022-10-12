using BasicGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

    var reply = await client.SendMessageAsync(
                  new ChatRequest
                  {
                      Name = name,
                      Message = message,
                      RequestStartTime = Timestamp.FromDateTime(DateTime.UtcNow)
                  });
    Console.WriteLine($"Reply: {reply.Message}");
    Console.WriteLine($"Answer found: {reply.AnswerFound}");

    var messageBytes = Encoding
        .UTF8
        .GetString(reply.ReplyInBytes.ToByteArray());

    Console.WriteLine($"Reply from bytes: {messageBytes}");
    Console.WriteLine($"Message size in bytes: {reply.MessageSizeInBytes}");
    Console.WriteLine($"Message size in megabytes: {reply.MessageSizeInMegabytes}");
    Console.WriteLine($"Request received time: {reply.RequestReceivedTime}");
    Console.WriteLine($"Request processed duration: {reply.RequestProcessedDuration}");
    Console.WriteLine($"Raw dynamic payload: {JsonSerializer.Serialize(reply.DynamicPayload)}");
    Console.WriteLine($"Unpacked dynamic payload: {JsonSerializer.Serialize(reply.DynamicPayload.Unpack<ChatRequest>())}");
}

Console.ReadKey();