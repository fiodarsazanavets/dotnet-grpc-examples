using BasicGrpcService;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public IndexModel()
    {
        Message = string.Empty;
        SecretMessage = string.Empty;
        OpenMessage = string.Empty;
    }

    public string Message { get; set; }

    public string SecretMessage { get; set; }
    
    public string OpenMessage { get; set; }

    public async Task OnGet()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        var metadata = new Metadata
        {
            { "Authorization", $"Bearer {accessToken}" }
        };

        using var channel = GrpcChannel.ForAddress("https://localhost:7100");
        var client = new MessageProcessor.MessageProcessorClient(channel);

        try
        {
            var response = await client.GetMessageAsync(new Request(), metadata);
            Message = response.Message;
        }
        catch { }

        try
        {
            var response = await client.GetSecretMessageAsync(new Request(), metadata);
            SecretMessage = response.Message;
        }
        catch { }

        try
        {
            var response = await client.GetOpenMessageAsync(new Request(), metadata);
            OpenMessage = response.Message;
        }
        catch { }
    }
}