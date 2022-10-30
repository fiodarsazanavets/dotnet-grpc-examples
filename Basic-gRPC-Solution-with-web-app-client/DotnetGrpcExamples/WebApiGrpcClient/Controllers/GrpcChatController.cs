using BasicGrpcService;
using Microsoft.AspNetCore.Mvc;

namespace WebApiGrpcClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrpcChatController : ControllerBase
    {
        private readonly Chatbot.ChatbotClient _client;

        public GrpcChatController(Chatbot.ChatbotClient client)
        {
            _client = client;
        }

        [HttpGet("{name}/{message}")]
        public IActionResult Get(string name, string message)
        {
            var reply = _client.SendMessage(new ChatRequest
            {
                Name = name,
                Message = message
            });

            return Ok(reply);
        }
    }
}