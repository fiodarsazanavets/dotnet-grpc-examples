using Google.Protobuf;
using Grpc.Core;
using System.Text;

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

        var reply = new ChatReply();

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
        reply.Payload = GetPayload();

        return Task.FromResult(reply);
    }

    private NumericPayload GetPayload()
    {
        return new NumericPayload
        {
            FloatDemo = 1.1F,
            DoubleDemo = 1.2,
            Int32Demo = int.MinValue,
            Int64Demo = long.MinValue,
            Uint32Demo = uint.MaxValue,
            Uint64Demo = ulong.MaxValue,
            Sint32Demo = int.MinValue,
            Sint64Demo = long.MinValue,
            Fixed32Demo = uint.MaxValue,
            Fixed64Demo = ulong.MaxValue,
            Sfixed32Demo = int.MaxValue,
            Sfixed64Demo = long.MaxValue,
        };
    }
}