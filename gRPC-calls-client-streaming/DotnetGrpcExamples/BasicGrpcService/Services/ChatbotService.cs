using Grpc.Core;

namespace BasicGrpcService.Services;

public class ChatbotService : Chatbot.ChatbotBase
{
    private readonly ILogger<ChatbotService> _logger;
    public ChatbotService(ILogger<ChatbotService> logger)
    {
        _logger = logger;
    }

    public override async Task<ChatReply> SendMessages(IAsyncStreamReader<ChatRequest> requestStream, ServerCallContext context)
    {
        _logger.LogDebug("Message received from the client {Peer}.", context.Peer);

        var replyMessage = string.Empty;

        await foreach (var request in requestStream.ReadAllAsync())
        {
            if (request.Message.ToLower().Contains("hello") || request.Message.ToLower().Contains("came from"))
                replyMessage += $"Hello {request.Name}! ";
            else if (request.Message.ToLower().Contains("help"))
                replyMessage += "What can I help you with today? ";
            else
                replyMessage += "I'm sorry, but I am unable to resolve your query. ";
        }

        return new ChatReply
        {
            Message = replyMessage
        };
    }
}