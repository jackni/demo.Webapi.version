using Demo.DataAccess;
using Demo.DataAccess.Entities;
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
        public IAsyncEnumerable<Todo> ListAll()
        {
            return _dbContext.Todos.ToAsyncEnumerable();
        }

        public async Task Add(Todo todo)
        {
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();
        }
    }
}
