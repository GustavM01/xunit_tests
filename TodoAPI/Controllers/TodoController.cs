using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Data;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("/notes")]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext database;

        public TodoController(TodoContext database)
        {
            this.database = database;
        }


        [HttpGet]
        [Route("/notes")]
        public ActionResult<IEnumerable<Todo>> GetNotes(bool? completed = null)
        {
            IQueryable<Todo> notes = database.Todo;
            if (completed.HasValue)
            {
                notes = notes.Where(n => n.IsDone == completed.Value);
            }
            return notes.ToArray();
        }

        [HttpGet]
        [Route("/remaining")]
        public int GetRemaining()
        {
            return database.Todo.Where(n => n.IsDone == false).Count();
        }


        [HttpPost]
        public ActionResult<Todo> PostNote(Todo note)
        {
            var newNote = new Todo
            {
                IsDone = note.IsDone,
                Text = note.Text,
                Description = note.Description
            };

            database.Todo.Add(newNote);
            database.SaveChanges();

            return CreatedAtAction(nameof(GetNotes), new { id = newNote.Id }, newNote);
        }

        [HttpPut("{id}")]
        public void ChangeStatus(int id)
        {
            var todoToChange = database.Todo.Find(id);

            if (todoToChange.IsDone == true)
            {
                todoToChange.IsDone = false;
            }
            else
            {
                todoToChange.IsDone = true;
            }
            database.SaveChanges();
        }

        [HttpPost]
        [Route("/toggle-all")]
        public void ToggleAll()
        {
            if (database.Todo.All(n => n.IsDone == true))
            {
                foreach (Todo todo in database.Todo)
                {
                    todo.IsDone = false;
                }
            }
            else
            {
                foreach (Todo todo in database.Todo)
                {
                    todo.IsDone = true;
                }
            }
            database.SaveChanges();
        }


        [HttpDelete("{id}")]
        public void DeleteNote(int id)
        {
            var note = database.Todo.Find(id);
            database.Todo.Remove(note);
            database.SaveChanges();
        }


        [HttpPost]
        [Route("/clear-completed")]
        public void ClearCompleted()
        {
            foreach (Todo todo in database.Todo)
            {
                if (todo.IsDone == true)
                {
                    database.Todo.Remove(todo);
                }
            }
            database.SaveChanges();
        }
    }
}