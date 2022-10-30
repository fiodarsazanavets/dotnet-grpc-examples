using BasicGrpcService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, o => o.Protocols =
        HttpProtocols.Http1);

    options.ListenAnyIP(5100, o => o.Protocols =
        HttpProtocols.Http2);

    options.ListenAnyIP(7100, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

app.MapGrpcService<ChatbotService>();
app.MapGet("/", () => "gRPC service is running.");

app.Run();
