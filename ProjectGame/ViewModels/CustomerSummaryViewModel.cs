namespace ProjectGame.ViewModels;

public class CustomerSummaryViewModel
{
    public string Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastOrderDate { get; set; }
}