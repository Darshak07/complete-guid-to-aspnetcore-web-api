using my_books.Data.Models;

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
    }
}
