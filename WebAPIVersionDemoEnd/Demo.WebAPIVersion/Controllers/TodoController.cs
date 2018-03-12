using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.DataAccess.Entities;
using Demo.Domain;
using Demo.WebAPIVersion.Models;
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
        [Route("ListAllAsync")]
        [ApiExplorerSettings(GroupName = "Todo demo")]
        [ProducesResponseType(typeof(TodoItem),200)]
        public async Task<IActionResult> ListAllAsync()
        {
            var data = await _todoRepo.ListAll();
            var result = data.Select(d =>
                new TodoItem {
                    Id = d.Id, Name = d.Name,
                    Description = d.Description,
                    CreatedDateTime = d.CreatedDateTime.ToLocalTime()
                });
            return Ok(result);
        }

        [HttpPost]
        [Route("AddAsync")]
        [ApiExplorerSettings(GroupName = "Todo demo")]
        public async Task<IActionResult> AddAsync([FromBody]TodoItem todo)
        {
            if(!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return BadRequest(message);
            }
            var todo_add = new Todo
            {
                Name = todo.Name,
                Description = todo.Description,
                CreatedDateTime = DateTime.UtcNow
            };
            await _todoRepo.Add(todo_add);
            //it should be 201
            return Ok();
        }
    }
}