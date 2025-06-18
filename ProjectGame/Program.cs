using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectGame.Data;
using ProjectGame.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Cookie
builder.Services.ConfigureApplicationCookie(options =>
    {
        // 當未登入/未授權嘗試訪問受保護頁面時，自動跳轉的 URL
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";

        // 登入逾時
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

        // 有操作就重新計時
        options.SlidingExpiration = true;
    });

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddRazorPages();

builder.Services.AddSession();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



// 初始化Role
async Task SeedRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { SD.Role_Admin, SD.Role_Staff, SD.Role_Customer };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

// HomePage
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// Update Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try { await SeedRoles(services); } catch (Exception ex) { Console.WriteLine($"Error SeedRoles: {ex.Message}"); }
    try { await PlatformSeedData.InitializeAsync(services); } catch (Exception ex) { Console.WriteLine($"Error PlatformSeed: {ex.Message}"); }
    try { await GameSeedData.InitializeAsync(services); } catch (Exception ex) { Console.WriteLine($"Error GameSeed: {ex.Message}"); }
    try { await UserSeedData.InitializeAsync(services); } catch (Exception ex) { Console.WriteLine($"Error UserSeed: {ex.Message}"); }
    try { await OrderSeedData.SeedAsync(services); } catch (Exception ex) { Console.WriteLine($"Error OrderSeed: {ex.Message}"); }
}


app.Run();
