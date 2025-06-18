namespace ProjectGame.Models;

public class ShoppingCartItem
{
    public int Id { get; set; }
    public int ShoppingCartId { get; set; }
    public ShoppingCart ShoppingCart { get; set; }
    public int GameID { get; set; }
    public Game Game { get; set; }
    public int Quantity { get; set; }
}
