using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAngularStorm.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApiAngularStorm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TodosController : Controller
    {
        private readonly ApiContext _context;

        public TodosController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Todos.ToList());
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(int id)
        {
            var todo = _context.Todos.SingleOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound($"No todo with an Id of {id} was found.");
            }
            return Ok(todo);
        }
        public IActionResult Post([FromBody] Todo todo)
        {
            if (string.IsNullOrEmpty(todo.Description))
            {
                return BadRequest("There must be a description in the todo.");
            }

            _context.Entry(todo).State = todo.Id > 0 ? EntityState.Modified : EntityState.Added;
            _context.SaveChanges();
            return CreatedAtRoute("GetTodo", new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound($"No todo with an Id of {id} was found.");
            }

            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return Ok();
        }

    }
}
