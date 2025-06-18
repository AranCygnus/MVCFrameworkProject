using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Data;
using ProjectGame.Models;
using ProjectGame.ViewModels;

namespace ProjectGame.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // GET: Home/Animation
    public IActionResult Animation()
    {
        return View();
    }


    // GET: Home/Index
    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {

            var referer = HttpContext.Request.Headers["Referer"].ToString();
            if (!referer.Contains("/Home/Animation"))
            {
                return RedirectToAction("Animation");
            }
        }

        var recentGames = await _context.Games.OrderByDescending(g =>g.ReleaseDate).Take(9).ToListAsync();
    
        return View(recentGames);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
