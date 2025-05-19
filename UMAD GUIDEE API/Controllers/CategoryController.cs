using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareProject.Models;
using UMAD_GUIDEE_API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UMAD_GUIDEE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public CategoryController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<List<Category>> Get()
        {
            var categories = await _dataContext.Categories.ToListAsync();

            return categories;
        }


        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var result = await _dataContext.Categories.FindAsync(id);

            if ( result == null )
            {
                return BadRequest();
            }

            return Ok(result);
        }

        //[Authorize(Roles = "Admin")]
        // POST api/<CategoryController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Category>> Post([FromBody] Category cat)
        {
            if ( !ModelState.IsValid )
            {
                return BadRequest(cat);
            }

            try
            {
                await _dataContext.Categories.AddAsync(cat);
                await _dataContext.SaveChangesAsync();

                return Ok(cat);
            }
            catch ( Exception ex )
            {
                return BadRequest(ex);
            }
        }

        //[Authorize(Roles = "Admin")]
        // PUT api/<CategoryController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category cat)
        {
                try
                {
                    _dataContext.Entry(cat).State = EntityState.Modified;

                    await _dataContext.SaveChangesAsync();
                }
                catch ( Exception ex )
                {
                    return NotFound(ex.Message);
                }

                return Ok(cat);
            }

        //[Authorize(Roles = "Admin")]
        // DELETE api/<CategoryController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(int id)
        {
            var category = await _dataContext.Categories.FindAsync(id);
            if ( category == null )
            {
                return BadRequest();
            }
            try
            {
                _dataContext.Categories.Remove(category);
                await _dataContext.SaveChangesAsync();

                return Ok(category);
            }
            catch ( Exception ex )
            {
                return BadRequest(ex);
            }
        }
    }
}
