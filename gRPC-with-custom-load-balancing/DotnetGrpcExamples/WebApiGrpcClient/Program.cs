using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using System.Net;
using WebApiGrpcClient;
using static WebApiGrpcClient.RandmomizedBalancer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ResolverFactory, CsvResolverFactory>();
builder.Services.AddSingleton<LoadBalancerFactory, RandomizedBalancerFactory>();

builder.Services.AddGrpcClient<Chatbot.ChatbotClient>(o =>
{
    o.Address = new Uri("csv://addresses.csv");
})
.ConfigureChannel(o =>
{
    o.Credentials = ChannelCredentials.Insecure;
    o.DisableResolverServiceConfig = true;
    o.ServiceConfig = new ServiceConfig
    {
        LoadBalancingConfigs = {
            new LoadBalancingConfig("randomized")
        }
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