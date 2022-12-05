using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebApi.DbOperations;
using WebApi.Entities;

namespace WebApi.DBOperations
{
    public class DataGenerator   //bu uygulama ayağa kalktığında başlangıcta verilerimiz olsun istiyoruz ondan bu sınıfı açtık
    {
        //IServiceProvider inmemory database ile alakalı Program.cs kendi içindeki ServiceProvider ile burayı  çağırarak uygulama hep ilk ayağa kalktığında hep çalısıcak bir yapı yapıcaz 
        public static void Initialize(IServiceProvider serviceProvider) //DataGenerator.Initialize(services); program.cs bunu(services) bu(serviceProvider) parametrenin yerine koydu bağlantı kurdu
        {
            //yaratmış olduğumuz context in bir tane instance ına ihitiyacımız var..çünkü database ye kaydedicez o bilgileri bunu da context aracılıgı ile yaparız
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))  //GetRequiredService => DependencyInjection
            {
                //bu scope içerisinde context nesnesini kullanabilirsin anlamında yukarıdaki
                if (context.Books.Any())
                {
                    return;
                }
                context.Genres.AddRange(
                    new Genre
                    {
                        Name = "Personal Growth"
                    },
                     new Genre
                    {
                        Name = "Science Fiction"
                    },
                     new Genre
                    {
                        Name = "Romance"
                    }

                    );

                context.Books.AddRange(  //Book un DB de karşılığı Books oraya eklemler yaptık uygulama ayağa kalktıgında 
                     new Book
                     {
                         
                         Title = "Lean Startup",
                         GenreId = 1, 
                         PageCount = 200,
                         PublishDate = new DateTime(2011, 06, 12)

                     },
                     new Book
                     {
                        
                         Title = "Herland",
                         GenreId = 2, 
                         PageCount = 250,
                         PublishDate = new DateTime(2010, 05, 23)

                     },
                     new Book
                     {
                         
                         Title = "Dune",
                         GenreId = 2,
                         PageCount = 540,
                         PublishDate = new DateTime(1995, 08, 25)

                     }

                  );

                context.SaveChanges(); // database de işlem yaptıktan sonra bunu yapıyoruz db ye yazılmasını sağlıyoruz...
            }

        }

    }
}
