using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Domain;
using Demo.WebAPIVersion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPIVersion.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    //[Route("api/Math")]
    [Route("api/{version:apiVersion}/[controller]")]
    public class MathController : Controller
    {
        private readonly ISimpleMath _simpleMath;

        public MathController(ISimpleMath simpleMath)
        {
            _simpleMath = simpleMath;
        }

        [HttpPost]
        [Route("Add")]
        [ApiExplorerSettings(GroupName = "Math 1.0")]
        public IActionResult Add([FromBody]CalcuatorModel calcuatorRequest)
        {
            return Ok(_simpleMath.Add(calcuatorRequest.X, calcuatorRequest.Y));
        }
    }
}