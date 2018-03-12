using Demo.DataAccess;
using Demo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DemoContext _dbContext;

        public TodoRepository(DemoContext demoContext)
        {
            _dbContext = demoContext;
        }
        public async Task<IEnumerable<Todo>> ListAll()
        {
            return await _dbContext.Todos.ToListAsync();
        }

        public async Task Add(Todo todo)
        {
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();
        }
    }
}
