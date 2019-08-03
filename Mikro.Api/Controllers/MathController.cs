using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mikro.Api.Repositories;
using Mikro.Messages.Commands;
using RawRabbit;

namespace Mikro.Api.Controllers
{
    [Route("[controller]")]
    public class MathController : ControllerBase
    {
        private IBusClient _client;
        private IRepository _repository;
        
        public MathController(IBusClient client,IRepository repository)
        {
            _client = client;
            _repository=repository;
        }
        // GET api/values
        [HttpGet("")]
        public IActionResult Get()
        {
            return Content("Math Controller");
        }
        [HttpGet("{number}")]
        public IActionResult Get(int number)
        {
            int? result = _repository.Get(number);
            if (result.HasValue)
            {
                return Content(result.ToString());
            }

            return Content("Not ready...");
        }
        // POST api/values
        [HttpPost("{number}")]
        public async Task<IActionResult>  Post(int number)
        {
            int? result = _repository.Get(number);
            if (!result.HasValue)
            {
                await _client.PublishAsync(
                    new CalculateValueCommand
                    {
                        Number = number
                    });
            }

            return Accepted($"Math/{number}", null);
        }
    }
}