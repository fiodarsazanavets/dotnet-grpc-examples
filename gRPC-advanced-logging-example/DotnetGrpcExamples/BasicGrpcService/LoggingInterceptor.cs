using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace BasicGrpcService;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> _logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor>  logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        LogCall(context);

        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall(context);

        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall(context);

        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        LogCall(context);

        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            LogException(ex);
            throw;
        }
    }

    private void LogCall(ServerCallContext context)
    {
        _logger.LogDebug($"gRPC request: {context.GetHttpContext().Request.Path}");
    }

    private void LogException(Exception ex)
    {
        _logger.LogError(ex, "gRPC error occurred");
    }
}
