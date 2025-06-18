using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Data;
using ProjectGame.Models;
using System;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;

namespace ProjectGame.Controllers;

// [Authorize(Roles = SD.Role_Customer)] // 僅Customer可使用
public class StoreController : Controller
{
    private readonly AppDbContext _context;

    public StoreController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Store/Index
    public async Task<IActionResult> Index(string sSearch, string PlatformID, int? page)
    {
        ViewData["CurrentFilter"] = sSearch;
        ViewData["CurrentPlatform"] = PlatformID;
        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name");

        // Page number setting
        const int pageSize = 16;
        int pageNumber = page ?? 1;

        // Initial query with includes
        var query = _context.Games.Include(g => g.Platform).AsQueryable();

        // Select Game Platform
        if (!string.IsNullOrEmpty(PlatformID) && PlatformID != "Select")
        {
            if (int.TryParse(PlatformID, out int pid))
            {
                query = query.Where(g => g.PlatformID == pid);
            }
        }

        // Search Game Title
        if (!string.IsNullOrEmpty(sSearch))
        {
            query = query.Where(g => g.Title.Contains(sSearch));
        }

        // ReleaseDate排序
        query = query.OrderByDescending(g => g.ReleaseDate);

        IPagedList<Game> pagedList = query.ToPagedList(pageNumber, pageSize);

        return View(pagedList);
    }

    // GET: Store/Details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games.Include(g => g.Platform).FirstOrDefaultAsync(g => g.ID == id);

        if (game == null) return NotFound();

        return View(game);
    }


    // Action: Add To Cart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(int gameId, string platformId, string sSearch, int quantity = 1)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new ShoppingCart { UserId = userId, Items = new List<ShoppingCartItem>() };
            _context.ShoppingCarts.Add(cart);
        }

        var item = cart.Items.FirstOrDefault(i => i.GameID == gameId);
        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new ShoppingCartItem
            {
                GameID = gameId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Successfully added to Cart!";
        return RedirectToAction("Index", new { PlatformID = platformId, sSearch });
    }

    // Action: Cart Count
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            var cartCount = _context.ShoppingCartItems
                .Where(i => i.ShoppingCart.UserId == userId)
                .Sum(i => (int?)i.Quantity) ?? 0;

            ViewBag.CartCount = cartCount;
        }
    }


    // GET: Store/Cart
    public async Task<IActionResult> Cart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || cart.Items.Count == 0)
        {
            TempData["WarningMessage"] = "The Cart is Empty!";
            return View(new List<ShoppingCartItem>());
        }
        return View(cart.Items);
    }

    // Action: Remove From Cart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(int gameId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        var item = cart?.Items.FirstOrDefault(i => i.GameID == gameId);
        if (item != null)
        {
            cart.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Cart");
    }

    // Action: Update Quantity
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(int gameId, int change)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        var item = cart?.Items.FirstOrDefault(i => i.GameID == gameId);
        if (item != null)
        {
            item.Quantity += change;
            if (item.Quantity <= 0)
                cart.Items.Remove(item);

            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Cart");
    }

    // Action: Clear Cart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearCart()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart != null)
        {
            _context.ShoppingCartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
        }

        TempData["WarningMessage"] = "Empty the Shopping Cart!";
        return RedirectToAction("Cart");
    }


    // GET: Store/Confirmation
    [Route("Store/Confirmation/{orderId}")]
    public IActionResult Confirmation(int orderId)
    {
        ViewData["OrderId"] = orderId;
        return View();
    }


    // GET: Store/Checkout
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            TempData["ErrorMessage"] = "The Cart is Empty, can't Checkout!";
            return RedirectToAction("Cart");
        }
        return View(cart.Items);
    }


    // GET: Store/CheckoutConfirm
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CheckoutConfirm()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.ShoppingCarts
            .Include(c => c.Items)
            .ThenInclude(i => i.Game)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.Items.Any())
        {
            TempData["ErrorMessage"] = "The Cart is Empty, can't Checkout!";
            return RedirectToAction("Cart");
        }

        var order = new Order
        {
            UserID = userId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.Unpaid,
            OrderDetails = cart.Items.Select(item => new OrderDetail
            {
                GameID = item.GameID,
                Quantity = item.Quantity,
                UnitPrice = item.Game.Price
            }).ToList()
        };
        _context.Orders.Add(order);

        _context.ShoppingCartItems.RemoveRange(cart.Items);
        await _context.SaveChangesAsync();

        return RedirectToAction("Confirmation", new { orderId = order.ID });
    }


    // GET: Store/MarkPaid
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkPaid(int orderId, string cardHolder, string cardNumber, string expiration, string cvv)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null || order.Status != OrderStatus.Unpaid) return NotFound();

        order.Status = OrderStatus.Paid;
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Payment successful!";
        return RedirectToAction("History");
    }

    // Action: Cancel Order
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.ID == orderId);

        if (order == null || order.Status != OrderStatus.Unpaid) return NotFound();

        order.Status = OrderStatus.Cancelled;
        await _context.SaveChangesAsync();

        TempData["InfoMessage"] = "Order Cancelled";
        return RedirectToAction("History");
    }


    // GET: Store/Payment
    [HttpGet]
    public async Task<IActionResult> Payment(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null || order.Status != OrderStatus.Unpaid) return NotFound();

        ViewData["OrderId"] = orderId;
        return View();
    }


    // GET: Store/History
    public async Task<IActionResult> History()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var orders = await _context.Orders
            .Where(o => o.UserID == userId)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Game)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }

    // GET: Store/OrderDetail
    public async Task<IActionResult> OrderDetail(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(d => d.Game)
            .FirstOrDefaultAsync(o => o.ID == id);

        if (order == null)
            return NotFound();

        return View(order);
    }




    // Action: MarkD elivered

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDelivered(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null || order.Status != OrderStatus.Shipped)
        {
            return NotFound();
        }

        order.Status = OrderStatus.Completed;
        _context.Update(order);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Order marked as completed.";
        return RedirectToAction(nameof(History));
    }
}
