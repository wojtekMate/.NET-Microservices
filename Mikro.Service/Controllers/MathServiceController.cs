using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mikro.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathServiceController : ControllerBase
    {

        // GET api/values/5
        [HttpGet("")]
        public ActionResult<string> Get(int id)
        {
            return "Service Controller";
        }

    }
}
