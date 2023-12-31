﻿using Microsoft.AspNetCore.Identity;
using my_books.Data.Models;
using my_books.Data.ViewModels.Authentication;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(new Book()
                    {
                        Title = "Naruto Shippuden",
                        Description = "About changing life Hard work etc...",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(-10),
                        Rate= 4,
                        Genre = "Autography",
                       
                        CoverUrl = "gogoAnime",
                        DateAdded = DateTime.Now
                    },
                    new Book()
                    {
                        Title = "RichDad PoorDad",
                        Description = "About not HardWorking and earning money etc...",
                        IsRead = false,
                        Genre = "Money",
                       
                        CoverUrl = "YouTube",
                        DateAdded = DateTime.Now
                    },
                     new Book()
                     {
                         Title = "RichDad PoorDad V2",
                         Description = "About not HardWorking and earning money etc...",
                         IsRead = false,
                         Genre = "Money",
                         
                         CoverUrl = "YouTube",
                         DateAdded = DateTime.Now
                     });

                    context.SaveChanges();
                }
            }
        }
        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.Publisher))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Publisher));
                if (!await roleManager.RoleExistsAsync(UserRoles.Author))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Author));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            }
        }
    }
}
