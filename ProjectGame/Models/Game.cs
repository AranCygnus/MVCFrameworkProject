using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectGame.Models;

public class Game
{
    public int ID { get; set; }

    [Required]
    [StringLength(100)]
    public required string Title { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(10, 0)")]
    [DisplayFormat(DataFormatString = "{0:C0}")]
    public decimal Price { get; set; }

    [MaxLength]
    [ValidateNever]
    [Column(TypeName = "varbinary(MAX)")]
    public byte[]? DataFiles { get; set; } = null;

    public int PlatformID { get; set; }
    public Platform? Platform { get; set; }
}