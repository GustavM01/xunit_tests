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

        [Fact]
        public async void Get_all_todos()
        {
            // Arrange
            await ResetContext();
            var todo1 = new Todo { Id = 1, Text = "Sleep", IsDone = false };
            var todo2 = new Todo { Id = 2, Text = "Work", IsDone = false };
            var service = new TodoService(_context);
            service.PostNote(todo1);
            service.PostNote(todo2);

            // Act
            var result = service.GetNotes(null);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains(result, x => x.Text == todo1.Text);
            Assert.Contains(result, x => x.Text == todo2.Text);
        }

        [Fact]
        public async void Delete_todo_check_if_deleted()
        {
            // Arrange
            await ResetContext();
            var todo = new Todo 
            { 
                Id = 1, 
                Text = "Eat", 
                IsDone = true 
            };
            var service = new TodoService(_context);
            var addedTodo = service.PostNote(todo);

            // Act
            var result = service.DeleteNote(addedTodo.Id);

            // Assert
            Assert.Equal(result.Text, todo.Text);

            // Check that the todo is deleted
            Assert.Empty(_context.Todo);
        }

    }
}