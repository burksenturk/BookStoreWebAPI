using Microsoft.EntityFrameworkCore;
using System;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.DbOperations           //DB operasyonlar�m�zda kullanmmak i�in bir tane DBContext dosyas�na ihtiyac�m�z var..Bu context dosya�m�z�n arac�l�g� ile 
{                                          //db nesnelerimizin replicalar�na eri�icez temel CRUD i�lemlerimizi bu context arac�l��� ile yap�caz
    public class BookStoreDbContext : DbContext,IBookStoreDbContext //KALITIM VERD� BU A�AMA UN�T TESTTE
    {
        //Constructor   //DbContextOptions ayarlar�n� BookStoreDbContext ile yapacaks�n.. : base(options) DbContext den ald�g�n kal�t�msal �zelliklerle
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) //private readonly BookStoreDbContext _context;
        { }

        public DbSet<Book> Books { get; set; }  //bu DB de Book Entitysini (nesnesini) kullanmak istiyorum ismide Books olsun.. DATABASE DE yarat�lacak isim Books olacak genelde �ogul olur
        public DbSet<Genre> Genres{ get; set; }  //resourcelerimiz hep �o�ul olur.
        public DbSet<User> Users { get; set; }

        public override int SaveChanges() //TEK �STED���M BOOKSTORDBCONTEXT �ZER�NDEN ER���LEB�L�N�R OLMASI BU Y�ZDEN B�YLE B�R TAKLA ATTIRIYPORUZ BURADA (UN�T)
        {
            return base.SaveChanges();   //base = DbContext
        }


    }
}


