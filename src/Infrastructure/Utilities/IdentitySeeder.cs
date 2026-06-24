using Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Utilities;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        await SeedRoles(roleManager);
        await SeedAdmin(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole<Guid>> roleManager)
    {
        string[] roles =
        {
            "Admin",
            "Student",
            "Instructor"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }

    private static async Task SeedAdmin(UserManager<User> userManager)
    {
        var email = "admin@site.com";

        var admin = await userManager.FindByEmailAsync(email);

        if (admin == null)
        {
            admin = new User
            {
                UserName = email,
                Email = email,
                FirstName = "Admin",
                LastName = "site.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin_123");

            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}