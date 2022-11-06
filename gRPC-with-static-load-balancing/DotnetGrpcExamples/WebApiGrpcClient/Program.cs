using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.AspNetCore.Server.Kestrel.Core;

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