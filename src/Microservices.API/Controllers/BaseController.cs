using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using Newtonsoft.Json;
using Microservices.Domain.Models;
using System.Text;
using Microservices.Domain.Interfaces;
using Microservices.Application;

namespace Microservices.API.Controllers
{
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        public readonly ConnectionFactory _factory;
        public const string QUEUE_NAME = "#QUEUE_NAME#";

        public BaseService<BaseModel> _service;

        public BaseController(BaseService<BaseModel> service)
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _service = service;
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] BaseModel message)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,  
                        arguments: null);

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage);
                }
            }

            return Accepted();
        }


    }
}
