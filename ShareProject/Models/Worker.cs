/// Clase generada solo para los trabajadores para que tengan solo ellos ligados las notas creadas

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareProject.Models;

public class Worker 
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }

    
    public ICollection<Note> Notes { get; set; } = [];
}
