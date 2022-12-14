using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BasicGrpcService.Services;

[Authorize]
public class MessageProcessorService : MessageProcessor.MessageProcessorBase
{
    private readonly ILogger<MessageProcessorService> _logger;
    public MessageProcessorService(ILogger<MessageProcessorService> logger)
    {
        _logger = logger;
    }

    public override Task<Reply> GetMessage(Request request, ServerCallContext context)
    {
        return Task.FromResult(GenerateReply("Normal message."));
    }

    [Authorize(Roles = "admin")]
    public override Task<Reply> GetSecretMessage(Request request, ServerCallContext context)
    {
        return Task.FromResult(GenerateReply("Secret message."));
    }

    [AllowAnonymous]
    public override Task<Reply> GetOpenMessage(Request request, ServerCallContext context)
    {
        return Task.FromResult(GenerateReply("Open message."));
    }

    private Reply GenerateReply(string replyMessage)
    {
        return new Reply
        {
            Message = replyMessage
        };
    }
}