using BasicGrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.MapGrpcService<ChatbotService>();
app.MapGet("/", () => "gRPC service is running.");

app.Run();
