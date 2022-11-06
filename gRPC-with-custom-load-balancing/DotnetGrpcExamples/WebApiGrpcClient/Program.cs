using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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