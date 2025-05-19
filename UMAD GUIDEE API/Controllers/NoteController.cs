using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareProject.Models;
using ShareProject.Models.DTOs;
using ShareProject.Models.DTOs.OutputDTO;
using UMAD_GUIDEE_API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UMAD_GUIDEE_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NoteController(DataContext dataContext) : ControllerBase
    {
        private readonly DataContext _dataContext = dataContext;


        // GET: api/<NoteController>
        [HttpGet]
        public async Task<ActionResult<List<NoteOutputDTO>>> GetAllNotes()
        {
            var notes = await ( from note in _dataContext.Notes.Include(b => b.Worker).Include(b => b.Catergory)
                                select new NoteOutputDTO
                                {
                                    Id = note.Id,
                                    Title = note.Title,
                                    Description = note.Description,
                                    WorkerId = note.Worker.Id,
                                    WorkerFullName = $"{note.Worker.User.Name} {note.Worker.User.LastName}",
                                    CategoryId = note.Catergory.Id,
                                    CategoryName = note.Catergory.Name
                                } ).ToListAsync();

            return notes;
        }

        /*
        // GET api/<NoteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NoteDTO>> Get(int id)
        {
            var result = await _dataContext.Notes.Include(category => category.Catergory).Include(user => user.User).FirstOrDefaultAsync(n => n.Id == id);

            if ( result == null )
            {
                return BadRequest();
            }

            var noteDto = new NoteDTO
            {
                Title = result.Title,
                Description = result.Description,
                Username = result.User.Name,
                CategoryName = result.Catergory.Name
            };

            return Ok(noteDto);
        }
        */

        [Authorize(Roles = "Teacher")]
        // POST api/<NoteController>
        [HttpPost]
        public async Task<ActionResult<NoteOutputDTO>> Post([FromBody] NoteOutputDTO note)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest();
            }

            //var worker = await _dataContext.Workers.FindAsync(note.WorkerId);

            // Verificamos y enlazamos el id con el del Worker
            var worker = await _dataContext.Workers.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Email == this.User.Identity.Name);

            if ( worker == null )
            {
                return NotFound("Worker not found for the logged in user.");
            }

            var category = await _dataContext.Categories.FindAsync(note.CategoryId);

            if ( category == null )
            {
                return NotFound("Category not found.");
            }

            Note newNote = new()
            {
                Title = note.Title,
                Description = note.Description,
                Worker = worker,
                Catergory = category
            };

            await dataContext.Notes.AddAsync(newNote);

            await dataContext.SaveChangesAsync();

            NoteOutputDTO newNoteOutput = new()
            {
                Title = newNote.Title,
                Description = newNote.Description,
                WorkerFullName = $"{newNote.Worker.User.Name} {newNote.Worker.User.LastName}",
                CategoryName = newNote.Catergory.Name
            };

            return Ok(newNoteOutput);

        }

        [Authorize (Roles = "Teacher")]
        // PUT api/<NoteController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<NoteOutputDTO>> Put(int id, [FromBody] NoteOutputDTO noteUpdate)
        {
            if ( !ModelState.IsValid )
                return BadRequest();

            if ( id != noteUpdate.Id )
                return BadRequest();

            // Obtener el Id
            var userId = User.FindFirst("Id")?.Value;

            // Verificamos si el Id es nulo
            if ( userId == null )
            {
                return Unauthorized("User not found or invalid token.");
            }

            var worker = await _dataContext.Workers.Include(w => w.User).FirstOrDefaultAsync(w => w.User.Id == userId);

            if ( worker == null )
            {
                return NotFound("Worker not found for the logged in user.");
            }

            // Modificamos para enlazar el id del Worker
            //var note = await dataContext.Notes.FirstOrDefaultAsync(b => b.Id == id);

            var note = await dataContext.Notes.Include(b => b.Worker).FirstOrDefaultAsync(b => b.Id == id);

            if ( note == null )
                return NotFound();

            var category = await _dataContext.Categories.FindAsync(noteUpdate.CategoryId);

            if ( category == null )
                return NotFound("Category not found.");

            note.Id = noteUpdate.Id;
            note.Title = noteUpdate.Title;
            note.Description = noteUpdate.Description;
            note.Worker = worker;
            note.Catergory = category;

            dataContext.Notes.Update(note);
            await dataContext.SaveChangesAsync();

            NoteOutputDTO newNote = new()
            {
                Title = note.Title,
                Description = note.Description,
                WorkerFullName = $"{note.Worker.User.Name} {note.Worker.User.LastName}",
                CategoryName = note.Catergory.Name
            };

            return Ok(newNote);
        }

        [Authorize(Roles = "Admin")]
        // DELETE api/<NoteController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NoteOutputDTO>> Delete(int id)
        {
            var note = await dataContext.Notes.FirstOrDefaultAsync(b => b.Id == id);

            if ( note == null )
                return NotFound();

            dataContext.Notes.Remove(note);

            await dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
