using Contracts;
using ProtoBuf.Grpc;

namespace BasicGrpcService.Services;

public class ChatbotService : IChatbotService
{
    public Task<ChatReply> SendMessage(ChatRequest request, 
        CallContext context = default)
    {
        string? replyMessage;

        if (request.Message.ToLower().Contains("hello"))
            replyMessage = $"Hello {request.Name}!";
        else if (request.Message.ToLower().Contains("help"))
            replyMessage = "What can I help you with today?";
        else
            replyMessage = "I'm sorry, but I am unable to resolve your query.";

        return Task.FromResult(new ChatReply
        {
            Message = replyMessage
        });
    }
}