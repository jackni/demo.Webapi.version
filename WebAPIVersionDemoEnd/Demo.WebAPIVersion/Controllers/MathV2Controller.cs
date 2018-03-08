using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPIVersion.Controllers
{
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    public class MathV2Controller : Controller
    {
        private readonly ISimpleMathV2 _simpleMathV2;
        public MathV2Controller(ISimpleMathV2 simpleMathV2)
        {
            _simpleMathV2 = simpleMathV2;
        }

        [HttpPost]
        [Route("Sum")]
        [ApiExplorerSettings(GroupName = "Math 2.0")]
        public IActionResult Add([FromBody] float[] numbers)
        {
            return Ok(_simpleMathV2.Sum(numbers));
        }
    }
}