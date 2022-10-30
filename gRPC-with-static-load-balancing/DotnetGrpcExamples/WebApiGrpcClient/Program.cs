using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var factory = new StaticResolverFactory(addr => new[]
{
    new BalancerAddress("localhost", 5100)
});

builder.Services.AddSingleton<ResolverFactory>(factory);

builder.Services.AddGrpcClient<Chatbot.ChatbotClient>(o =>
{
    o.Address = new Uri("static:///example-host");
})
.ConfigureChannel(o =>
{
    o.Credentials = ChannelCredentials.Insecure;
    o.DisableResolverServiceConfig = true;
    o.ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = { new RoundRobinConfig() }
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();