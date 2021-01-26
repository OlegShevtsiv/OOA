using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Data
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string UserAdminEmail = "oleg28skm@gmail.com";
            string UserAdminPassword = "_Aa123456UserAdmin";

            string LibraryAdminEmail = "LibAdmin@gmail.com";
            string LibraryAdminPassword = "_Aa123456LibraryAdmin";

            string SuperAdminEmail = "SuperAdmin@gmail.com";
            string SuperAdminPassword = "SuperAdmin123.";


            string UserAdminRole = "user admin";
            string LibraryAdminRole = "library admin";
            string UserRole = "user";
            if (await roleManager.FindByNameAsync(UserAdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserAdminRole));
            }
            if (await roleManager.FindByNameAsync(LibraryAdminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(LibraryAdminRole));
            }
            if (await roleManager.FindByNameAsync(UserRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(UserRole));
            }
            if (await userManager.FindByNameAsync(UserAdminEmail) == null)
            {
                IdentityUser UserAdmin = new IdentityUser { UserName = "OlegShevtsivUserAdmin", Email = UserAdminEmail};
                IdentityResult result = await userManager.CreateAsync(UserAdmin, UserAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(UserAdmin, UserAdminRole);
                }
            }
            if (await userManager.FindByNameAsync(LibraryAdminEmail) == null)
            {
                IdentityUser LibraryAdmin = new IdentityUser { UserName = "JhonSmowLibraryAdmin", Email = LibraryAdminEmail};
                IdentityResult result = await userManager.CreateAsync(LibraryAdmin, LibraryAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(LibraryAdmin, LibraryAdminRole);
                }
            }

            if (await userManager.FindByNameAsync(SuperAdminEmail) == null)
            {
                IdentityUser SuperAdmin = new IdentityUser { UserName = "SuperAdmin", Email = SuperAdminEmail};
                IdentityResult result = await userManager.CreateAsync(SuperAdmin, SuperAdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(SuperAdmin, LibraryAdminRole);
                    await userManager.AddToRoleAsync(SuperAdmin, UserAdminRole);
                    await userManager.AddToRoleAsync(SuperAdmin, UserRole);
                }
            }
        }
    }
}
