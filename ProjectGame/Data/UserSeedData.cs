using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Models;

namespace ProjectGame.Data;

public static class UserSeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();


        context.Database.Migrate();


        // Create User
        var users = new[]
        {
            new { Name = "Hyman", Email = "Hocxbrr6501@yahoo.com", Password = "gx6d-nQ8N-Wykn", EmailConfirmed = true},
            new { Name = "Toby", Email = "Txsfdb3518@gmail.com",  Password = "ZpM4-Ibbp-9o6H", EmailConfirmed = true},
            new { Name = "Simon", Email = "Sjqapqwt9792@gmail.com",  Password = "mR6l-ggcc-DrQ7", EmailConfirmed = true},
            new { Name = "Justin", Email = "Jljy4285@hotmail.com",  Password = "rSzM-1QlS-BSJt" , EmailConfirmed = true},
            new { Name = "Karen", Email = "Knvwuq5224@gmail.com", Password = "w8ZB-t2G8-tTfk" , EmailConfirmed = true},
            new { Name = "Quincy", Email = "Qgbkmgg4150@gmail.com", Password = "RgjU-fQfX-XgHA" , EmailConfirmed = true},
            new { Name = "Amelia", Email = "Adczcv2597@gmail.com", Password = "rema-zjp0-6yjY" , EmailConfirmed = true},
            new { Name = "Vincent", Email = "Vxobvpo1567@hotmail.com", Password = "iYVm-ssFy-YNan" , EmailConfirmed = true},
            new { Name = "Xaviera", Email = "Xeqqrbx8921@gmail.com", Password = "nKhD-zvEL-N8va" , EmailConfirmed = true},
            new { Name = "Geraldine", Email = "Ggemsba0195@gmail.com", Password = "Esrj-bOR2-AukV" , EmailConfirmed = true},
            new { Name = "Wade", Email = "Wfozse6020@hotmail.com", Password = "71t0-qDvb-erJU" , EmailConfirmed = true},
            new { Name = "Spencer", Email = "Skbbdvul5617@gmail.com", Password = "7Ket-TOCc-doxP" , EmailConfirmed = true},
            new { Name = "Tess", Email = "Tatrj6281@gmail.com", Password = "gJPq-Fvh3-dbVz" , EmailConfirmed = true},
            new { Name = "Josephine", Email = "Jncd0820@hotmail.com", Password = "e6MV-fYVZ-1ajA" , EmailConfirmed = true},
            new { Name = "Zachary", Email = "Zqhxv2148@gmail.com", Password = "inuE-cf3i-1ZhY" , EmailConfirmed = true},
            new { Name = "Myron", Email = "Mmghzal3589@hotmail.com", Password = "zsaL-9kaU-nNhJ" , EmailConfirmed = true}
        };

        var userIds = new Dictionary<string, string>();

        foreach (var u in users)
        {
            var existingUser = await userManager.FindByEmailAsync(u.Email);
            if (existingUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = u.Email,
                    Email = u.Email,
                    Name = u.Name,
                    EmailConfirmed = u.EmailConfirmed
                };

                var result = await userManager.CreateAsync(newUser, u.Password);
                if (result.Succeeded)
                {
                    userIds[u.Email] = newUser.Id;
                }else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"❌ 建立使用者 {u.Email} 失敗：{error.Description}");
                    }
                }
            }
            else
            {
                userIds[u.Email] = existingUser.Id;
            }
        }

        await context.SaveChangesAsync();
        
    }
}
