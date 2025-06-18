using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectGame.Models;

namespace ProjectGame.Data;

public static class PlatformSeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        if (await context.Platforms.AnyAsync())
            return;

        var platforms = new List<Platform>
        {
            new () { Name = "PS5", Description = "PlayStation 5" },
            new () { Name = "NS", Description = "Nintendo Switch" },
            new () { Name = "Xbox", Description = "Xbox Series X" }
        };

        await context.Platforms.AddRangeAsync(platforms);
        await context.SaveChangesAsync();
    }
}