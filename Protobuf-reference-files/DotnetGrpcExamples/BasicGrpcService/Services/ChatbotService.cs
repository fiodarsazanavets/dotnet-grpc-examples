using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
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

    public override Task<Response.ChatReply> SendMessage(ChatRequest request, ServerCallContext context)
    {
        _logger.LogDebug("Message received from the client {Peer}.", context.Peer);

        var reply = new Response.ChatReply();

        if (request.Message.ToLower().Contains("hello"))
        {
            reply.Message = $"Hello {request.Name}!";
            reply.AnswerFound = true;
        }
        else if (request.Message.ToLower().Contains("help"))
        {
            reply.Message = "What can I help you with today?";
            reply.AnswerFound = true;
        }
        else
        {
            reply.Message = "I'm sorry, but I am unable to resolve your query.";
        }

        reply.ReplyInBytes = 
            ByteString.CopyFrom(
                Encoding.ASCII.GetBytes(reply.Message));

        reply.MessageSizeInBytes = reply.ReplyInBytes.Length;
        reply.MessageSizeInMegabytes = (double)reply.MessageSizeInBytes / (1024 * 1024);
        reply.RequestReceivedTime = Timestamp.FromDateTime(DateTime.UtcNow);
        reply.RequestProcessedDuration = reply.RequestReceivedTime - request.RequestStartTime;
        reply.DynamicPayload = Any.Pack(request);

        _historyStore.AddEntry(request.Message, reply.Message);

        return Task.FromResult(reply);
    }
}