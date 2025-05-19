using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareProject.Models.DTOs.OutputDTO
{
    public class LoggedInOutputDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string Email { get; set; }

        public required string Role { get; set; }
    }
}
