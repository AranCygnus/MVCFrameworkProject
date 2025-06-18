using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectGame.Models;

public class Order
{
    public int ID { get; set; }

    [Required]
    public required string UserID { get; set; }

    public ApplicationUser User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    public ICollection<OrderDetail> OrderDetails { get; set; }

    public OrderStatus Status { get; set; }
}



