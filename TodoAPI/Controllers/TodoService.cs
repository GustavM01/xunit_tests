using TodoAPI.Data;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    public class TodoService
    {
        private TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public Todo PostNote(Todo todo)
        {
            if (string.IsNullOrWhiteSpace(todo.Text) || todo is null)
            {
                throw new NullReferenceException("Invalid todo");
            }

            Todo newTodo = new Todo { Text = todo.Text, IsDone = todo.IsDone };
            _context.Todo.Add(newTodo);
            _context.SaveChanges();

            return newTodo;
        }

    }
}
