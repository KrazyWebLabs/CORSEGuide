using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareProject.Models;
using ShareProject.Models.DTOs.InputDTO;
using ShareProject.Models.DTOs.OutputDTO;
using UMAD_GUIDEE_API.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UMAD_GUIDEE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(DataContext dataContext, UserManager<User> userManager) : ControllerBase
    {
        private readonly DataContext dataContext = dataContext;

        private readonly UserManager<User> userManager = userManager;

        // GET: api/<StudentController>
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<ActionResult<List<Student>>> Get()
        {
            return Ok(await dataContext.Students.Include(u => u.User).ToListAsync());
        }

        // GET api/<StudentController>/5
        [Authorize(Roles = "Teacher")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StudentController>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Student>> Post([FromBody] StudentInputDTO studentOutput)
        {
            var User = new User
            {
                Name = studentOutput.FistName,
                LastName = studentOutput.LastName,
                Email = studentOutput.Email,
                UserName = studentOutput.Email,
            };

            var createUser = await userManager.CreateAsync(User, studentOutput.Password);

            if (!createUser.Succeeded )
            {
                var errors = string.Join(", ", createUser.Errors.Select(e => e.Description));
                return BadRequest($"Error creating user: {errors}");
            }

            await userManager.AddToRoleAsync(User, "Student");

            var student = new Student
            {
                User = User
            };

            dataContext.Students.Add(student);
            await dataContext.SaveChangesAsync();

            return Ok(student);
        }


        // PUT api/<StudentController>/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StudentController>/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
