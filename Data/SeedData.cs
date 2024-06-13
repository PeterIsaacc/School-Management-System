// SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolManagementSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRolesAsync(roleManager);
            await SeedSchoolsAsync(context);
            await SeedActivitiesAsync(context);
            await SeedUsersAsync(userManager, roleManager, context);
            await SeedUserRolesAsync(userManager, roleManager, context);
        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new Role { Name = "Super Admin" });
                await roleManager.CreateAsync(new Role { Name = "School Admin" });
                await roleManager.CreateAsync(new Role { Name = "Activity Admin" });
            }
        }

        private static async Task SeedSchoolsAsync(ApplicationDbContext context)
        {
            if (!context.Schools.Any())
            {
                context.Schools.AddRange(
                    new School { SchoolName = "School A" },
                    new School { SchoolName = "School B" },
                    new School { SchoolName = "School C" }
                );
                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedActivitiesAsync(ApplicationDbContext context)
        {
            if (!context.Activities.Any())
            {
                var schoolA = context.Schools.FirstOrDefault(s => s.SchoolName == "School A");
                var schoolB = context.Schools.FirstOrDefault(s => s.SchoolName == "School B");
                var schoolC = context.Schools.FirstOrDefault(s => s.SchoolName == "School C");

                context.Activities.AddRange(
                    new Activity { ActivityName = "Activity 1", SchoolId = schoolA.Id },
                    new Activity { ActivityName = "Activity 2", SchoolId = schoolA.Id },
                    new Activity { ActivityName = "Activity 3", SchoolId = schoolB.Id },
                    new Activity { ActivityName = "Activity 4", SchoolId = schoolC.Id }
                );

                await context.SaveChangesAsync();
            }
        }

        private static async Task SeedUsersAsync(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            if (!userManager.Users.Any())
            {
                var superAdmin = new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(superAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    var role = await roleManager.FindByNameAsync("Super Admin");
                    if (role != null)
                    {
                        await userManager.AddToRoleAsync(superAdmin, role.Name);
                    }
                }
            }
        }

        private static async Task SeedUserRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager, ApplicationDbContext context)
        {
            if (!context.UserRoles.Any())
            {
                var superAdmin = await userManager.FindByNameAsync("admin");
                var superAdminRole = await roleManager.FindByNameAsync("Super Admin");

                if (superAdmin != null && superAdminRole != null)
                {
                    context.UserRoles.Add(new UserRole
                    {
                        UserId = superAdmin.Id,
                        RoleId = superAdminRole.Id
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}