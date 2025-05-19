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
    public class UserController(UserManager<User> userManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager;


        // GET: api/<UserController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<LoggedInOutputDTO>> GetLoggedInUser()
        {
            User? loggedInUser = await _userManager.GetUserAsync(User); // El user loggeado

            var roles = await _userManager.GetRolesAsync(loggedInUser);
            var currentRole = roles.FirstOrDefault(r => User.IsInRole(r));

            return Ok(new LoggedInOutputDTO
            {
                FirstName = loggedInUser.Name,
                LastName = loggedInUser.LastName,
                Role = currentRole.ToString(),
                Email = loggedInUser.Email
            });
        }
    }
}
