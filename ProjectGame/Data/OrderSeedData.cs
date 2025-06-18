using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Models;

namespace ProjectGame.Data;

public static class OrderSeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (!context.Orders.Any())
        {
            var allEmails = new[]
            {
                "Hocxbrr6501@yahoo.com",
                "Sjqapqwt9792@gmail.com",
                "Zqhxv2148@gmail.com",
                "Xeqqrbx8921@gmail.com",
                "Adczcv2597@gmail.com",
                "Ggemsba0195@gmail.com",
                "Knvwuq5224@gmail.com"
            };

            var userIds = new Dictionary<string, string>();
            foreach (var email in allEmails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    userIds[email] = user.Id;
                }
                else
                {
                    Console.WriteLine($"⚠️ 找不到使用者：{email}");
                }
            }


            var games = await context.Games.ToListAsync();
            if (games.Count < 30)
            {
                Console.WriteLine($"❌ Game 資料不足，目前僅有 {games.Count} 筆。需要至少 30 筆資料。");
                return;
            }

            var ordersToAdd = new List<Order>();

            void TryAddOrder(string email, DateTime date, OrderStatus status, List<(int gameIndex, int qty)> items)
            {
                if (!userIds.TryGetValue(email, out var userId))
                {
                    Console.WriteLine($"⚠️ 無法為 {email} 建立訂單，因為找不到此使用者");
                    return;
                }

                var order = new Order
                {
                    UserID = userId,
                    OrderDate = date,
                    Status = status,
                    OrderDetails = items.Select(i => new OrderDetail
                    {
                        GameID = games[i.gameIndex].ID,
                        Quantity = i.qty,
                        UnitPrice = games[i.gameIndex].Price
                    }).ToList()
                };

                ordersToAdd.Add(order);
            }

            


            TryAddOrder("Hocxbrr6501@yahoo.com", DateTime.Now.AddDays(-7).AddHours(15).AddMinutes(30), OrderStatus.Unpaid, new() { (10,1), (3,2) });
            TryAddOrder("Hocxbrr6501@yahoo.com", DateTime.Now.AddDays(-7).AddHours(17).AddMinutes(21), OrderStatus.Paid, new() { (5,1) });
            TryAddOrder("Sjqapqwt9792@gmail.com", DateTime.Now.AddDays(-10).AddHours(21).AddMinutes(15), OrderStatus.Cancelled, new() { (30,3) });
            TryAddOrder("Sjqapqwt9792@gmail.com", DateTime.Now.AddDays(-3).AddHours(8).AddMinutes(45), OrderStatus.Unpaid, new() { (28,1), (29,1), (19,1) });
            TryAddOrder("Xeqqrbx8921@gmail.com", DateTime.Now.AddDays(-1).AddHours(22).AddMinutes(51), OrderStatus.Unpaid, new() { (13,1), (23,1) });
            TryAddOrder("Adczcv2597@gmail.com", DateTime.Now.AddDays(-5).AddHours(12).AddMinutes(16), OrderStatus.Cancelled, new() { (1,3) });
            TryAddOrder("Xeqqrbx8921@gmail.com", DateTime.Now.AddDays(-15).AddHours(3).AddMinutes(2), OrderStatus.Completed, new() { (14,1), (16,2), (24,1), (26,1), (29,3) });
            TryAddOrder("Zqhxv2148@gmail.com", DateTime.Now.AddDays(-20).AddHours(1).AddMinutes(31), OrderStatus.Completed, new() { (33,1), (36,1), (34,1) });
            TryAddOrder("Ggemsba0195@gmail.com", DateTime.Now.AddDays(-2).AddHours(7).AddMinutes(32), OrderStatus.Paid, new() { (24,1), (22,2) });
            TryAddOrder("Ggemsba0195@gmail.com", DateTime.Now.AddDays(-8).AddHours(9).AddMinutes(45), OrderStatus.Completed, new() { (2,1), (3,1), (8,1), (5,1), (1,1) });
            TryAddOrder("Ggemsba0195@gmail.com", DateTime.Now.AddDays(-11).AddHours(10).AddMinutes(2), OrderStatus.Completed, new() { (15,1), (18,2), (23,1), (32,1) });
            TryAddOrder("Knvwuq5224@gmail.com", DateTime.Now.AddDays(-1).AddHours(9).AddMinutes(5), OrderStatus.Unpaid, new() { (3,3) });
            TryAddOrder("Knvwuq5224@gmail.com", DateTime.Now.AddDays(-3).AddHours(16).AddMinutes(43), OrderStatus.Shipped, new() { (14,1), (16,1), (30,1), (25,1), (22,1) });
            TryAddOrder("Qgbkmgg4150@gmail.com", DateTime.Now.AddDays(-2).AddHours(10).AddMinutes(24), OrderStatus.Paid, new() { (31,1), (29,1), (22,1), (15,1), (30,1) });
            TryAddOrder("Qgbkmgg4150@gmail.com", DateTime.Now.AddDays(-8).AddHours(11).AddMinutes(28), OrderStatus.Completed, new() { (22,1), (27,1) });
            TryAddOrder("Mmghzal3589@hotmail.com", DateTime.Now.AddDays(-1).AddHours(12).AddMinutes(15), OrderStatus.Cancelled, new() { (33,1), (36,1), (35,1), (37,1), (38,1) });



            context.Orders.AddRange(ordersToAdd);
            await context.SaveChangesAsync();
            Console.WriteLine($"✅ 共建立 {ordersToAdd.Count} 筆訂單");
        }
    }
}