using ProtoBuf.Grpc;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Contracts;

[DataContract]
public class ChatRequest
{
    [DataMember(Order = 1)]
    public string Name { get; set; }
    [DataMember(Order = 2)]
    public string Message { get; set; }
}

[DataContract]
public class ChatReply
{
    [DataMember(Order = 1)]
    public string Message { get; set; }
}

[ServiceContract]
public interface IChatbotService
{
    [OperationContract]
    Task<ChatReply> SendMessage(ChatRequest request,
        CallContext context = default);
}