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
            await SeedManagerUser(userManager);
            await SeedEmployeeUser(userManager);
        }

        

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] {"Admin", "Manager", "Employee"};
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        //admin
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

        //repeat method for other roles



        //manager
        public static async Task SeedManagerUser(UserManager<IdentityUser> userManager)
        {
           
        }



        //employee
        public static async Task SeedEmployeeUser(UserManager<IdentityUser> userManager)
        {
            
        }


        


        


    }
}
