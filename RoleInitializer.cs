using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeshBrain.Models;

namespace LeshBrain
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<UserEntity> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "35agodar";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }
            if (await roleManager.FindByNameAsync("employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("employee"));
            }
            if(await roleManager.FindByNameAsync("mentor")==null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("mentor"));
            }
            if (await roleManager.FindByNameAsync("anon") == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>("anon"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                UserEntity admin = new UserEntity { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
        public static async Task InitializeAsync(UserManager<UserEntity> userManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "35agodar";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                UserEntity admin = new UserEntity { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
