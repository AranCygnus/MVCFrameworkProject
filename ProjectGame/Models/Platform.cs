using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectGame.Models;

public class Platform
{
    public int ID { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    public required string Description { get; set; }

    public ICollection<Game>? Games { get; set; }
}