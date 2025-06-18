using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectGame.Models;

public class ShoppingCart
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }

    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
}
