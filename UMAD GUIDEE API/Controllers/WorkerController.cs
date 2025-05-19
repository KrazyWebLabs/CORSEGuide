using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareProject.Models;
using ShareProject.Models.DTOs.OutputDTO;
using UMAD_GUIDEE_API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UMAD_GUIDEE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController(DataContext dataContext, UserManager<User> userManager) : ControllerBase
    {
        private readonly DataContext dataContext = dataContext;

        private readonly UserManager<User> userManager = userManager;

        // GET: api/<WorkerController>
        [HttpGet]
        public async Task<ActionResult<List<Worker>>> Get()
        {
            return Ok(await dataContext.Workers.Include(u => u.User).ToListAsync());
        }

        // GET api/<WorkerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<WorkerController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Worker>> Post([FromBody] WorkerOutputDTO workerOutput)
        {
            var User = new User
            {
                Name = workerOutput.FistName,
                LastName = workerOutput.LastName,
                Email = workerOutput.Email,
                UserName = workerOutput.Email
            };

            var createUser = await userManager.CreateAsync(User, "password123");
            
            if ( !createUser.Succeeded )
            {
                var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest($"Error creating user: {errors}");
            }
            
            await userManager.AddToRoleAsync(User, "Teacher");
            
            var worker = new Worker
            {
                User = User
            
            };
            dataContext.Workers.Add(worker);
            
            await dataContext.SaveChangesAsync();
            
            return Ok(worker);
        }

        // PUT api/<WorkerController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WorkerController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
