using BasicGrpcService;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGrpcClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrpcChatController : ControllerBase
    {
        private readonly MessageProcessor.MessageProcessorClient _client;

        public GrpcChatController(MessageProcessor.MessageProcessorClient client)
        {
            _client = client;
        }

        [HttpGet("message")]
        public async Task<IActionResult> GetMessage()
        {
            try
            {
                var response = await _client.GetMessageAsync(new Request(), await GetMetadata());
                return Ok(response.Message);
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpGet("secret-message")]
        public async Task<IActionResult> GetSecretMessage()
        {
            try
            {
                var response = await _client.GetSecretMessageAsync(new Request(), await GetMetadata());
                return Ok(response.Message);
            }
            catch
            {
                return Forbid();
            }
        }

        [HttpGet("open-message")]
        public async Task<IActionResult> GetOpenMessage()
        {
            var response = await _client.GetOpenMessageAsync(new Request());
            return Ok(response.Message);
        }

        private async Task<Metadata> GetMetadata()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new Metadata
            {
                {
                    "Authorization",
                    $"Bearer {accessToken}"
                }
            };
        }
    }
}