using BasicGrpcService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders(
                "Grpc-Status", 
                "Grpc-Message",
                "Grpc-Encoding",
                "Grpc-Accept-Encoding");
}));

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5100, o => o.Protocols =
        HttpProtocols.Http1AndHttp2);

    options.ListenAnyIP(7100, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        listenOptions.UseHttps();
    });
});

var app = builder.Build();

app.UseGrpcWeb();
app.UseCors();

app.MapGrpcService<ChatbotService>()
    .EnableGrpcWeb()
    .RequireCors("AllowAll");
app.MapGet("/", () => "gRPC service is running.");

app.Run();
