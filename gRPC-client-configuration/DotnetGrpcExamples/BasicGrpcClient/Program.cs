using BasicGrpcClient;
using BasicGrpcService;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;

Console.WriteLine("What is your name?");
var name = Console.ReadLine();

Console.WriteLine($"Hello {name}. You can start chatting now.");
Console.WriteLine("Type 'exit' to stop at any time.");

var options = new GrpcChannelOptions
{
    HttpHandler = null,
    HttpClient = null,
    DisposeHttpClient = false,
    LoggerFactory = LoggerFactory.Create(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Debug);
    }),
    MaxSendMessageSize = null,
    MaxReceiveMessageSize = null,
    Credentials = null,
    CompressionProviders = null,
    ThrowOperationCanceledOnCancellation = false,
    UnsafeUseInsecureChannelCallCredentials = false,
    MaxRetryAttempts = 5,
    MaxRetryBufferSize = null,
    MaxRetryBufferPerCallSize = null,
    ServiceConfig = null
};

using var channel = GrpcChannel.ForAddress("http://localhost:5100", options);
var callInvoker = channel.Intercept(new TraceInterceptor());
var client = new Chatbot.ChatbotClient(callInvoker);

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
                      Message = message
                  });
    Console.WriteLine($"Reply: {reply.Message}");
}

Console.ReadKey();