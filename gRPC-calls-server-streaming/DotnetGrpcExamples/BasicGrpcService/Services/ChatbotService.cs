using Grpc.Core;

namespace BasicGrpcService.Services;

public class ChatbotService : Chatbot.ChatbotBase
{
    private readonly ILogger<ChatbotService> _logger;
    public ChatbotService(ILogger<ChatbotService> logger)
    {
        _logger = logger;
    }

    public override async Task SendMessage(ChatRequest request, IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
    {
        _logger.LogDebug("Message received from the client {Peer}.", context.Peer);

        string? replyMessage;

        if (request.Message.ToLower().Contains("hello"))
            replyMessage = $"Hello {request.Name}!";
        else if (request.Message.ToLower().Contains("help"))
            replyMessage = "What can I help you with today?";
        else
            replyMessage = "I'm sorry, but I am unable to resolve your query.";

        await responseStream.WriteAsync(new ChatReply
        {
            Message = replyMessage
        });

        await responseStream.WriteAsync(new ChatReply
        {
            Message = "What else can I help you with?"
        });
    }
}