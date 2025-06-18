using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectGame.Models;

namespace ProjectGame.Data;

public static class GameSeedData
{
    private static byte[] SafeReadImage(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
        return File.Exists(path) ? File.ReadAllBytes(path) : Array.Empty<byte>();
    }


    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();

        if (await context.Games.AnyAsync())
            return;


        var games = new List<Game>()
        {
            // 
            new Game
            {
                Title = "The Legend of Zelda: Breath of the Wild",
                ReleaseDate = new DateTime(2017, 03, 03),
                Price = 1550,
                DataFiles = SafeReadImage("ZeldaBreath.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "The Legend of Zelda: Tears of the Kingdom",
                ReleaseDate = new DateTime(2023, 05, 12),
                Price = 1850,
                DataFiles = SafeReadImage("ZeldaKingom.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Legend of Zelda: Echoes of Wisdom",
                ReleaseDate = new DateTime(2024, 09, 26),
                Price = 1690,
                DataFiles = SafeReadImage("ZeldaEchoes.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Super Smash Bros. Ultimate",
                ReleaseDate = new DateTime(2018, 12, 07),
                Price = 1790,
                DataFiles = SafeReadImage("SuperSmashSP.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Pikmin 4",
                ReleaseDate = new DateTime(2023, 07, 21),
                Price = 1790,
                DataFiles = SafeReadImage("Pikmin4.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Kirby and the Forgotten Land",
                ReleaseDate = new DateTime(2022, 03, 25),
                Price = 1690,
                DataFiles = SafeReadImage("KirbyForgottenLand.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Animal Crossing: New Horizons",
                ReleaseDate = new DateTime(2020, 03, 20),
                Price = 1550,
                DataFiles = SafeReadImage("AnimalCrossing.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Story of Seasons: Grand Bazaar",
                ReleaseDate = new DateTime(2025, 05, 22),
                Price = 1990,
                DataFiles = SafeReadImage("StoryofSeasons.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "FANTASY LIFE i: The Girl Who Steals Time",
                ReleaseDate = new DateTime(2025, 08, 29),
                Price = 1590,
                DataFiles = SafeReadImage("FantasyLifei.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Super Mario Party Jamboree",
                ReleaseDate = new DateTime(2024, 10, 07),
                Price = 1590,
                DataFiles = SafeReadImage("SMPJamboree.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Mario Kart 8 Deluxe",
                ReleaseDate = new DateTime(2017, 04, 18),
                Price = 1590,
                DataFiles = SafeReadImage("MK8Deluxe.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "XenobladeX Definitive Edition",
                ReleaseDate = new DateTime(2025, 03, 20),
                Price = 1690,
                DataFiles = SafeReadImage("XenobladeXDE.jpg"),
                PlatformID = 2
            },
            new Game
            {
                Title = "Clubhouse Games: 51 Worldwide Classics",
                ReleaseDate = new DateTime(2020, 06, 05),
                Price = 1050,
                DataFiles = SafeReadImage("ClubhouseGames51.jpg"),
                PlatformID = 2
            },

            // PS5
            new Game
            {
                Title = "ASTRO BOT",
                ReleaseDate = new DateTime(2024, 09, 06),
                Price = 1790,
                DataFiles = SafeReadImage("AstroBot.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Marvel's Spider-Man 2",
                ReleaseDate = new DateTime(2023, 10, 20),
                Price = 1990,
                DataFiles = SafeReadImage("SpiderMan2.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Ghost of Tsushima Director's Cut",
                ReleaseDate = new DateTime(2024, 05, 17),
                Price = 1990,
                DataFiles = SafeReadImage("GhostTsushima.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "ELDEN RING",
                ReleaseDate = new DateTime(2022, 02, 25),
                Price = 1790,
                DataFiles = SafeReadImage("EldenRing.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Monster Hunter Wilds",
                ReleaseDate = new DateTime(2025, 02, 28),
                Price = 1890,
                DataFiles = SafeReadImage("MHWilds.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Rise of the Ronin",
                ReleaseDate = new DateTime(2024, 03, 22),
                Price = 1990,
                DataFiles = SafeReadImage("RiseoftheRonin.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Stellar Blade",
                ReleaseDate = new DateTime(2024, 04, 26),
                Price = 1990,
                DataFiles = SafeReadImage("StellarBlade.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Atelier Yumia: The Alchemist of Memories & the Envisioned Land",
                ReleaseDate = new DateTime(2025, 01, 21),
                Price = 1990,
                DataFiles = SafeReadImage("AtelierYumia.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "AI LIMIT Deluxe Edition",
                ReleaseDate = new DateTime(2025, 03, 27),
                Price = 1280,
                DataFiles = SafeReadImage("AILimit.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "DEATH STRANDING 2: ON THE BEACH",
                ReleaseDate = new DateTime(2025, 06, 26),
                Price = 1990,
                DataFiles = SafeReadImage("DeathStranding2.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "GRAN TURISMO 7",
                ReleaseDate = new DateTime(2022, 03, 04),
                Price = 1990,
                DataFiles = SafeReadImage("GT7.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Split Fiction",
                ReleaseDate = new DateTime(2025, 03, 06),
                Price = 1690,
                DataFiles = SafeReadImage("SplitFiction.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "DEATH STRANDING DIRECTOR'S CUT",
                ReleaseDate = new DateTime(2021, 09, 24),
                Price = 1490,
                DataFiles = SafeReadImage("DeathStranding.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Hogwarts Legacy",
                ReleaseDate = new DateTime(2023, 02, 10),
                Price = 1790,
                DataFiles = SafeReadImage("HogwartsLegacy.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "The Last of Us Part I Remastered",
                ReleaseDate = new DateTime(2022, 09, 02),
                Price = 1990,
                DataFiles = SafeReadImage("TheLastofUs1.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "The Last of Us Part II Remastered",
                ReleaseDate = new DateTime(2024, 01, 19),
                Price = 1490,
                DataFiles = SafeReadImage("TheLastofUs2.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Marvel's Spider-Man: Miles Morales",
                ReleaseDate = new DateTime(2020, 11, 12),
                Price = 1490,
                DataFiles = SafeReadImage("SpiderManMilesMorales.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "ELDEN RING NIGHTREIGN",
                ReleaseDate = new DateTime(2025, 05, 30),
                Price = 1190,
                DataFiles = SafeReadImage("EldenRingNightreigen.jpg"),
                PlatformID = 1
            },
            new Game
            {
                Title = "Wuchang: Fallen Feathers",
                ReleaseDate = new DateTime(2025, 07, 24),
                Price = 1790,
                DataFiles = SafeReadImage("WuchangFallenFeathers.jpg"),
                PlatformID = 1
            },

            // XboX
            new Game
            {
                Title = "Call of Duty: Black Ops 6",
                ReleaseDate = new DateTime(2024, 10, 25),
                Price = 2119,
                DataFiles = SafeReadImage("CoD.jpg"),
                PlatformID = 3
            },
            new Game
            {
                Title = "Forza Horizon 5",
                ReleaseDate = new DateTime(2021, 11, 19),
                Price = 1688,
                DataFiles = SafeReadImage("ForzaHorizon.jpg"),
                PlatformID = 3
            },
            new Game
            {
                Title = "Grand Theft Auto V",
                ReleaseDate = new DateTime(2021, 11, 11),
                Price = 1190,
                DataFiles = SafeReadImage("GTAV.jpg"),
                PlatformID = 3
            },
            new Game
            {
                Title = "Resident Evil 4",
                ReleaseDate = new DateTime(2023, 03, 24),
                Price = 1250,
                DataFiles = SafeReadImage("ResidentEvil4.jpg"),
                PlatformID = 3
            },
            new Game
            {
                Title = "The Callisto Protocol Standard Edition",
                ReleaseDate = new DateTime(2013, 12, 15),
                Price = 2379,
                DataFiles = SafeReadImage("TheCallistoProtocol.jpg"),
                PlatformID = 3
            },
            new Game
            {
                Title = "Assassin's Creed Shadows",
                ReleaseDate = new DateTime(2025, 03, 28),
                Price = 1999,
                DataFiles = SafeReadImage("ACShadows.jpg"),
                PlatformID = 3
            }
        };


        await context.Games.AddRangeAsync(games);
        await context.SaveChangesAsync();
    }

}