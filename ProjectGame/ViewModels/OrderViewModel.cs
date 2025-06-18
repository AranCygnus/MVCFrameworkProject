using ProjectGame.Models;

namespace ProjectGame.ViewModels;

public class OrderViewModel
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public string Email { get; set; }

}