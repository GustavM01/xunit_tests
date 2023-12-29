﻿using TodoAPI.Data;
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


        public Todo[] GetNotes(bool? completed)
        {
            if (completed == true)
            {
                return _context.Todo.Where(x => x.IsDone).ToArray();
            }
            else if (completed == false)
            {
                return _context.Todo.Where(x => !x.IsDone).ToArray();
            }
            else
            {
                return _context.Todo.ToArray();
            }
        }

        public Todo DeleteNote(int Id)
        {
            var todo = _context.Todo.FirstOrDefault(x => x.Id == Id);

            if (todo == null)
            {
                throw new Exception("");
            }

            _context.Remove(todo);
            _context.SaveChanges();
            return todo;
        }

        public Todo ChangeStatus(Todo changeTodo)
        {
            var todo = _context.Todo.FirstOrDefault(x => x.Id == changeTodo.Id);

            if (todo == null)
            {
                throw new Exception("");
            }

            todo.IsDone = changeTodo.IsDone;

            _context.SaveChanges();

            return todo;
        }
    }
}
