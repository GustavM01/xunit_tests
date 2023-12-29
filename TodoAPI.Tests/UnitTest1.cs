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

        [Fact]
        public async void Change_todo_status_returns_details()
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
            var result = service.ChangeStatus(new Todo { Id = addedTodo.Id, Text = "Eat", IsDone = false });

            // Assert
            Assert.False(result.IsDone);

            // Check that todo has changed in database
            Assert.False(_context.Todo.Single(x => x.Id == result.Id).IsDone);
        }


        [Fact]
        public async void Toggle_notes_return_opposite_details()
        {
            // Arrange
            await ResetContext();
            var todo1 = new Todo 
            { 
                Id = 1, 
                Text = "Eat", 
                IsDone = true 
            };
            var todo2 = new Todo 
            { 
                Id = 2, 
                Text = "Sleep", 
                IsDone = false 
            };
            var service = new TodoService(_context);
            service.PostNote(todo1);
            service.PostNote(todo2);

            // Act
            // Toggle once to complete all
            var result = await service.ToggleAll();

            // Assert
            Assert.All(result, x => Assert.True(x.IsDone));

            // Act
            // Toggle again to make all incomplete
            var result2 = await service.ToggleAll();

            // Assert
            Assert.All(result2, x => Assert.False(x.IsDone));
        }
    }
}