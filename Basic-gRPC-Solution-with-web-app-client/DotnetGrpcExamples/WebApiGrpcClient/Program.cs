using BasicGrpcService;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<Chatbot.ChatbotClient>(o =>
{
    o.Address = new Uri("http://localhost:5100");
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000, o => o.Protocols =
        HttpProtocols.Http1);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "gRPC Client");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();