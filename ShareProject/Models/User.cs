/// Clase que aloja los elementos necesarios de todos los usuarios, añadiendo los necesarios 
/// ademas de la clase IdentityUser que implementamos

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareProject.Models;

public class User : IdentityUser
{
    [MaxLength(500)]
    [Required]
    public string Name { get; set; }

    [MaxLength(500)]
    [Required]
    public string LastName { get; set; }

}
