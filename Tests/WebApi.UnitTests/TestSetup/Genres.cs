using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DbOperations;
using WebApi.Entities;

namespace WebApi.UnitTests.TestSetup
{
    public static class Genres  //initial data olusturduk
    {
        public static void AddGenres(this BookStoreDbContext context)
        {
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
        }
    }
}
