using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectGame.Models;

public class OrderDetail
{
    public int ID { get; set; }

    [Required]
    public int OrderID { get; set; }

    [Required]
    public int GameID { get; set; }

    public int Quantity { get; set; }

    public Order Order { get; set; }
    public Game Game { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }
}
