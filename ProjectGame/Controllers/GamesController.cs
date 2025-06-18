using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Data;
using ProjectGame.Models;
using ProjectGame.ViewModels;
using X.PagedList;
using X.PagedList.Extensions;


namespace ProjectGame.Controllers;


// [Authorize(Roles = SD.Role_Staff)]  // 僅Staff可使用
public class GamesController : Controller
{
    private readonly AppDbContext _context;
    string _path;
    private readonly UserManager<ApplicationUser> _userManager;

    public GamesController(AppDbContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _path = $@"{hostEnvironment.WebRootPath}\images";
        _userManager = userManager;
    }


    // GET: Games/Index
    public async Task<IActionResult> Index(string sSearch, string PlatformID, int? page, string sortOrder)
    {
        ViewData["CurrentFilter"] = sSearch;
        ViewData["CurrentPlatform"] = PlatformID;
        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name");

        // Initialize Sorting Parameters
        ViewData["CurrentSort"] = sortOrder;
        ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
        ViewData["ReleaseDateSortParm"] = sortOrder == "ReleaseDate" ? "date_desc" : "ReleaseDate";
        ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

        // Initial query with includes
        var query = _context.Games.Include(g => g.Platform).AsQueryable();

        // Default ReleaseDate Sort
        if (String.IsNullOrEmpty(sortOrder))
        {
            sortOrder = "";
        }

        // Sort Data
        switch (sortOrder)
        {
            case "Title":
                query = query.OrderBy(g => g.Title);
                break;
            case "title_desc":
                query = query.OrderByDescending(g => g.Title);
                break;
            case "date_desc":
                query = query.OrderBy(g => g.ReleaseDate);
                break;
            case "Price":
                query = query.OrderBy(g => g.Price);
                break;
            case "price_desc":
                query = query.OrderByDescending(g => g.Price);
                break;
            default:
                query = query.OrderByDescending(g => g.ReleaseDate);
                break;
        }

        // Page number setting
        const int pageSize = 10;
        int pageNumber = page ?? 1;

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

        IPagedList<Game> pagedList = query.ToPagedList(pageNumber, pageSize);

        return View(pagedList);
    }


    // GET: Games/Details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games
            .Include(g => g.Platform)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (game == null) return NotFound();

        return View(game);
    }


    // GET: Games/Create
    public IActionResult Create()
    {
        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name");
        return View();
    }

    // POST: Games/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ID,Title,PlatformID,ReleaseDate,Price,DataFiles")] Game game, IFormFile? DataFiles)
    {
        if (ModelState.IsValid)
        {
            // Check if the image file is provided
            if (DataFiles != null)
            {

                using (var memoryStream = new MemoryStream())
                {
                    await DataFiles.CopyToAsync(memoryStream);


                    // Restrict upload formats
                    game.DataFiles = memoryStream.ToArray();
                }
            }

            _context.Add(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name", game.PlatformID);

        return View(game);
    }


    // GET: Games/Edit
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();

        // Data Temporary Storage
        TempData["CurrentImageUrl"] = game.ID;
        TempData.Keep("CurrentImageUrl");

        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name", game.PlatformID);
        return View(game);
    }

    // POST: Games/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ID,Title,PlatformID,ReleaseDate,Price,DataFiles")] Game game, IFormFile? DataFiles)
    {
        if (id != game.ID) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existing = await _context.Games.FirstOrDefaultAsync(g => g.ID == id);
                if (existing == null) return NotFound();

                // Update fields
                existing.Title = game.Title;
                existing.PlatformID = game.PlatformID;
                existing.ReleaseDate = game.ReleaseDate;
                existing.Price = game.Price;

                if (DataFiles != null)
                {
                    using var memoryStream = new MemoryStream();
                    await DataFiles.CopyToAsync(memoryStream);
                    existing.DataFiles = memoryStream.ToArray();
                }

                // No need to call _context.Update
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(game.ID)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewData["PlatformID"] = new SelectList(_context.Platforms, "ID", "Name", game.PlatformID);
        return View(game);
    }

    // Action: File
    public async Task<IActionResult> GetImage(int? id)
    {
        if (id == null) return NotFound();
        
        var game = await _context.Games.FindAsync(id);
        if (game == null) return NotFound();

        return File(game.DataFiles, "image/jpeg");
    }


    // GET: Games/Delete
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games
            .Include(g => g.Platform)
            .FirstOrDefaultAsync(m => m.ID == id);
        if (game == null) return NotFound();

        return View(game);
    }


    // POST: Games/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool GameExists(int id)
    {
        return _context.Games.Any(e => e.ID == id);
    }


    // GET: Games/CustomerOrders
    public async Task<IActionResult> CustomerOrders(string status, int page = 1)
    {

        var statusList = Enum.GetValues(typeof(OrderStatus))
            .Cast<OrderStatus>()
            .Select(s => new SelectListItem
            {
                Text = s.ToString(),
                Value = s.ToString()
            })
            .Prepend(new SelectListItem { Text = "All", Value = "" })
            .ToList();

        var orders = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderDetails)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            var parsed = Enum.Parse<OrderStatus>(status);
            orders = orders.Where(o => o.Status == parsed);
        }

        var list = await orders
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderViewModel
            {
                OrderId = o.ID,
                Name = o.User.Name,
                Email = o.User.Email,
                OrderDate = o.OrderDate,
                Total = o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice),
                Status = o.Status
            })
            .ToListAsync();

        var pagedList = list.ToPagedList(page, 10);


        ViewData["CurrentFilter"] = status ?? "";
        ViewData["StatusList"] = statusList;

        return View(pagedList);
    }


    // Action: Mark Shipped
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkShipped(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return NotFound();

        order.Status = OrderStatus.Shipped;
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Order {orderId} marked as shipped.";
        return RedirectToAction("CustomerOrders");
    }


    // Action: Update Quantity
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        ViewBag.UnshippedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Paid);
        await next();
    }


    // GET: Games/CustomerOrderDetail
    public async Task<IActionResult> OrderDetails(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Game)
            .FirstOrDefaultAsync(o => o.ID == orderId);

        if (order == null) return NotFound();

        var user = await _userManager.FindByIdAsync(order.UserID);
        var email = user?.Email ?? "Unknown";

        var viewModel = new OrderDetailViewModel
        {
            OrderId = order.ID,
            UserEmail = email,
            OrderDate = order.OrderDate,
            Status = order.Status,
            Items = order.OrderDetails.Select(od => new OrderItemViewModel
            {
                GameTitle = od.Game.Title,
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList()
        };

        return View("CustomerOrderDetail", viewModel);
    }


    // GET: Games/CustomerInfo
    public async Task<IActionResult> CustomerInfo(string? searchName, int page = 1)
    {
        var users = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchName))
        {
            users = users.Where(u => u.Name.Contains(searchName));
        }

        var summaryQuery = users
            .GroupJoin
            (
                _context.Orders
                    .Include(o => o.OrderDetails),
                user => user.Id,
                order => order.UserID,
                (user, orders) => new
                {
                    user.Name,
                    user.Email,
                    Orders = orders
                }
            )
            .Select(g => new CustomerSummaryViewModel
            {
                Name = g.Name,
                Email = g.Email,
                TotalOrders = g.Orders.Count(),                            // 0 也算進來
                TotalSpent = g.Orders
                               .SelectMany(o => o.OrderDetails)
                               .Sum(d => d.Quantity * d.UnitPrice),
                LastOrderDate = g.Orders.Any()
                               ? g.Orders.Max(o => o.OrderDate)
                               : (DateTime?)null
            })
            .OrderByDescending(c => c.LastOrderDate ?? DateTime.MinValue);

        var list = await summaryQuery.ToListAsync();

        var pagedList = list.ToPagedList(page, 10);

        ViewData["CurrentFilter"] = searchName;

        return View(pagedList);
    }


    // GET: Games/CustomerOrderList
    public async Task<IActionResult> CustomerOrderList(string name, int page = 1)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        if (user == null) return NotFound();

        var orders = await _context.Orders
            .Where(o => o.UserID == user.Id)
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Game)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var pagedList = orders
            .Select(o => new OrderDetailViewModel
            {
                OrderId = o.ID,
                Name = user.Name,
                UserEmail = user.Email,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Items = o.OrderDetails.Select(d => new OrderItemViewModel
                {
                    GameTitle = d.Game.Title,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            })
            .ToPagedList(page, 5);

        ViewData["UserName"] = name;

        return View(pagedList);
    }



}

