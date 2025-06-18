using ProjectGame.Models;

namespace ProjectGame.ViewModels;

public class OrderDetailViewModel
{
    public int OrderId { get; set; }
    public string Name { get; set; }
    public string UserEmail { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
}