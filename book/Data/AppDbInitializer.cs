using book.Data.Models;

namespace book.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Book
                if (!context.Books.Any())
                {
                    context.Books.AddRange(new Book()
                            {
                                Title= "Title1",
                                Description= "Description1",
                                IsRead = true,
                                DateRead= DateTime.Now.AddDays(-10),
                                Rate=4,
                                Genre="Biography",
                                CoverUrl="url ",
                                DateAdded = DateTime.Now
                            },
                            new Book()
                            {
                                Title = "Title2",
                                Description = "Description2",
                                IsRead = false,
                                Genre = "Biography",
                                CoverUrl = "url ",
                                DateAdded = DateTime.Now
                            });
                    context.SaveChanges();
                }
            }

        }
    }
}
