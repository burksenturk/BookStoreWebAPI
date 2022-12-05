using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.DBOperations
{
    public interface IBookStoreDbContext
    {
        #region interface ve objeler hakkında bilgi
        //Unit Test yazmadan önce bizim bi tane dependencyimiz vardı Dbcontext controller da direkt inject oluyor Imapper gibi interface yapmalıyız bookstoredbcontexti de
        //o interface den türetmeyi yapıp projemizi bir bağımlılıktan daha kurtarmış olucaz.

        //bizim en çok kullandıklarımız entityler( book ve genre) bir de savechanges metodu. bunların objelerini yaratıcaz burada ve artık dışarıdan bu interface i implemente ettiğim
        //yerlerden bu objelere ulaşabiliyor olucaz 
        #endregion
        DbSet<Book> Books { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<User> Users { get; set; }


        int SaveChanges();



    }
}
