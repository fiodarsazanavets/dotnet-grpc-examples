using Grpc.Core;
using Grpc.Core.Interceptors;

namespace BasicGrpcClient;

internal class TraceInterceptor : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine(
            $"Starting a {context.Method.Type} call on {context.Method.Name} method.");
        return continuation(request, context);
    }
}
