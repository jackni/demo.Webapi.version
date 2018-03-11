using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.DataAccess.Entities;

namespace Demo.Domain
{
    public interface ITodoRepository
    {
        Task Add(Todo todo);
        IAsyncEnumerable<Todo> ListAll();
    }
}