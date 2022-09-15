using Google.Protobuf;
using Grpc.Core;
using System.Text;

namespace BasicGrpcService.Services;

public class ChatbotService : Chatbot.ChatbotBase
{
    private readonly ILogger<ChatbotService> _logger;
    private readonly IChatHistoryStore _historyStore;

    public ChatbotService(
        ILogger<ChatbotService> logger,
        IChatHistoryStore historyStore)
    {
        _logger = logger;
        _historyStore = historyStore;
    }

    public override Task<ChatReply> SendMessage(ChatRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Message received from the client {Peer}.", context.Peer);

        var reply = new ChatReply();

        if (request.Message.ToLower().Contains("hello"))
        {
            reply.Message = $"Hello {request.Name}!";
            reply.AnswerFound = true;
            reply.AnswerType = AnswerType.Greeting;
            reply.ResponseType = ChatHistoryEntry.Types.ResponseType.Greeting;
        }
        else if (request.Message.ToLower().Contains("help"))
        {
            reply.Message = "What can I help you with today?";
            reply.AnswerFound = true;
            reply.AnswerType = AnswerType.Help;
            reply.ResponseType = ChatHistoryEntry.Types.ResponseType.Assistance;
        }
        else
        {
            reply.Message = "I'm sorry, but I am unable to resolve your query.";
        }

        reply.ReplyInBytes = 
            ByteString.CopyFrom(
                Encoding.ASCII.GetBytes(reply.Message));

        foreach (var entry in _historyStore.GetHistory())
            reply.MessageHistory[entry.Key] = new ChatHistoryEntry
            {
                RequestMessage = entry.Value.Item1,
                ResponseMessage = entry.Value.Item2
            };

        _historyStore.AddEntry(request.Message, reply.Message);

        return Task.FromResult(reply);
    }
}