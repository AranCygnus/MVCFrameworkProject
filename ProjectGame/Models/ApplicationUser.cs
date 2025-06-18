using Microsoft.AspNetCore.Identity;

namespace ProjectGame.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}
