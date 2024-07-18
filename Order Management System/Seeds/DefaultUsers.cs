using Microsoft.AspNetCore.Identity;
using Order_Management_System.Contants;
using Order_Management_System.Core.Entities.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order_Management_System.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedCustomerUserAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "CustomerUser",
                Email = "CustomerUser@gmail.com",
                DisplayName = "Customer User",
                RoleName=Roles.Customer.ToString(),
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Customer.ToString());
            }
        }

        public static async Task SeedAdminUserAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "AdminUser",
                Email = "AdminUser@gmail.com",
                DisplayName = "Admin User"
                ,RoleName = Roles.Admin.ToString()
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "P@ssword123");
                await userManager.AddToRolesAsync(defaultUser, new List<string> { Roles.Admin.ToString(), Roles.Customer.ToString() });
            }
        }
    }
}
