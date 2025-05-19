using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareProject.Models;

public class Student
{
    [Key]
    public int Id { get; set; }

    public string Enrrolment { get; set; } = string.Empty;
    public User User { get; set; }
}
