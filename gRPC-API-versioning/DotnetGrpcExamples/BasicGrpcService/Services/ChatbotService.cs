using Grpc.Core;

namespace BasicGrpcService.Services;

public class ChatbotService : Chatbot.ChatbotBase
{
    private readonly ILogger<ChatbotService> _logger;
    public ChatbotService(ILogger<ChatbotService> logger)
    {
        _logger = logger;
    }

    public override Task<ChatReply> SendMessage(ChatRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Message received from the client {Peer}.", context.Peer);

        string? replyMessage;
        var replyFound = false;

        if (request.Message.ToLower().Contains("hello"))
        {
            replyMessage = $"Hello User!";
            replyFound = true;
        }
        else if (request.Message.ToLower().Contains("help"))
        {
            replyMessage = "What can I help you with today?";
            replyFound = true;
        }
        else
        {
            replyMessage = "I'm sorry, but I am unable to resolve your query.";
        }

        return Task.FromResult(new ChatReply
        {
            Message = replyMessage,
            ReplyFound = replyFound,
            RequestMessageLength = request.Message.Length,
            ResponseMessageLength = replyMessage.Length
        });
    }
}