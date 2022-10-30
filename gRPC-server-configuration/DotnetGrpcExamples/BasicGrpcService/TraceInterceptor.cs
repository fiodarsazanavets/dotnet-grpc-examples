using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BasicGrpcService;

public class TraceInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        Console.WriteLine(
            $"A {MethodType.Unary} call received on the {context.Method}.");
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error thrown by {context.Method}. {ex.Message}");
            throw;
        }
    }
}
