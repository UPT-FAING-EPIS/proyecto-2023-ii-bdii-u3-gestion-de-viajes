using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BitacoraController : ControllerBase
{
    private readonly RabbitMQClientService _rabbitMQClientService;

    public BitacoraController(RabbitMQClientService rabbitMQClientService)
    {
        _rabbitMQClientService = rabbitMQClientService;
    }

    [HttpPost]
    public IActionResult CreateLogEntry([FromBody] string message)
    {
        _rabbitMQClientService.Publish(message);
        return Ok();
    }
}

