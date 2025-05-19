using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareProject.Models;

public class Note
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(400)]
    public required string Description { get; set; }

    [ForeignKey("UserId")]
    public required Worker Worker { get; set; }

    [ForeignKey("CategoryId")]
    public required Category Catergory { get; set; }

    //public bool Priority { get; set; }
}
