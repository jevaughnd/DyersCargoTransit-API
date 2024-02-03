using DyersCargoTransit_API.Model;
using Microsoft.AspNetCore.Identity;

namespace DyersCargoTransit_API.Utils
{
    public class SeedData
    {
        public static async Task Intialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            await SeedRoles(roleManager);
            await SeedAdminUser(userManager);
            await SeedCustomerUser(userManager);

        }



        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        //Admin
        public static async Task SeedAdminUser(UserManager<IdentityUser> userManager)
        {
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                var admin = new IdentityUser()
                {
                    UserName = "admin",
                    Email = "admin@mail.com",
                };

                var createAdmin = await userManager.CreateAsync(admin, "Admin123%");

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }

            }
        }




        //Repeat method for other roles


        //Customer
        public static async Task SeedCustomerUser(UserManager<IdentityUser> userManager)
        {
            var customerUser = await userManager.FindByNameAsync("customer");

            if (customerUser == null)
            {
                var customer = new IdentityUser()
                {
                    UserName = "customer",
                    Email = "customer@mail.com",
                };

                var createCustomer = await userManager.CreateAsync(customer, "Customer123%");

                if (createCustomer.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, "Customer");
                }

            }
        }


    }

}


        


        


    

