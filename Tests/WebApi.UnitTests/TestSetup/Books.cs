using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DbOperations;
using WebApi.Entities;

namespace WebApi.UnitTests.TestSetup
{
    public static class Books  // nokta ile erişilsin dite static yaptık...test aşamasında boş DB gelmesin diye initial bir db olusutmak için yapıyoruz 
    {
        public static void AddBooks(this BookStoreDbContext context)
        {
                        context.Books.AddRange(   
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
        }
    }
}
