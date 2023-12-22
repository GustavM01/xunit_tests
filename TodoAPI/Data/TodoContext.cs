using TodoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Data
{
    public class TodoContext : DbContext
    {
        public DbSet<Todo> Todo { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }
    }
}