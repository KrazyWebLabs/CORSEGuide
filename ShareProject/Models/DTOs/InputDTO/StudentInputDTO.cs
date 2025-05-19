using ShareProject.Models.DTOs.OutputDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareProject.Models.DTOs.InputDTO
{
    public class StudentInputDTO : StudentOutputDTO
    {
        public string Password { get; set; }
    }
}
