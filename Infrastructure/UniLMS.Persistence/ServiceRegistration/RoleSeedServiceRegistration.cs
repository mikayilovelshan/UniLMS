using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class RoleSeedServiceRegistration
    {
        public static async Task SeedRolesAsync(this IHost app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roleNames = { "Admin", "Teacher", "Student" };

            foreach (var roleName in roleNames) 
            {
                    if (!await roleManager.RoleExistsAsync(roleName))
                        await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            } 

        }
    }
}
