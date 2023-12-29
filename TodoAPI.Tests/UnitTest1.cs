using TodoAPI.Controllers;
using TodoAPI.Data;
using TodoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Tests
{
    public class UnitTest1
    {
        private TodoContext _context;

        public UnitTest1()
        {
            var options = new DbContextOptionsBuilder<TodoContext>().UseInMemoryDatabase("InMemory").Options;
            _context = new TodoContext(options);
        }

        // Delete info
        private Task ResetContext()
        {
            _context.Todo.RemoveRange(_context.Todo);
            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }

        [Fact]
        public void Add_Todo_Check_if_saved_in_db()
        {
            // Arrange
            var todo = new Todo 
            { 
                Id = 111, 
                Text = "Eat", 
                IsDone = false 
            };

            // Service to handle requests
            var service = new TodoService(_context);

            // Act
            var result = service.PostNote(todo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todo.Text, result.Text);
            Assert.Equal(todo.IsDone, result.IsDone);

            // Assert that the todo is saved in the database
            _context.Todo.Single(x => x.Text == todo.Text);
        }
    }
}