using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.DataAccess.Entities;
using Demo.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebAPIVersion.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepo;

        public TodoController(ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
            return;
        }

        [HttpGet]
        [Route("ListAll")]
        [ApiExplorerSettings(GroupName = "Todo demo")]
        [ProducesResponseType(typeof(Todo),200)]
        public IActionResult ListAll()
        {
            return Ok(_todoRepo.ListAll());
        }
    }
}