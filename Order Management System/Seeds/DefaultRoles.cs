using Microsoft.AspNetCore.Identity;
using Order_Management_System.Contants;

namespace Order_Management_System.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> RoleManager)
        {

            if(!RoleManager.Roles.Any())
            {
                      await RoleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                      await RoleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
            }
            
        }

    }
}
