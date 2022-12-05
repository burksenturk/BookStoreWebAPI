using Microsoft.EntityFrameworkCore;
using System;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.DbOperations           //DB operasyonlarýmýzda kullanmmak için bir tane DBContext dosyasýna ihtiyacýmýz var..Bu context dosyaýmýzýn aracýlýgý ile 
{                                          //db nesnelerimizin replicalarýna eriþicez temel CRUD iþlemlerimizi bu context aracýlýðý ile yapýcaz
    public class BookStoreDbContext : DbContext,IBookStoreDbContext //KALITIM VERDÝ BU AÞAMA UNÝT TESTTE
    {
        //Constructor   //DbContextOptions ayarlarýný BookStoreDbContext ile yapacaksýn.. : base(options) DbContext den aldýgýn kalýtýmsal özelliklerle
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) //private readonly BookStoreDbContext _context;
        { }

        public DbSet<Book> Books { get; set; }  //bu DB de Book Entitysini (nesnesini) kullanmak istiyorum ismide Books olsun.. DATABASE DE yaratýlacak isim Books olacak genelde çogul olur
        public DbSet<Genre> Genres{ get; set; }  //resourcelerimiz hep çoðul olur.
        public DbSet<User> Users { get; set; }

        public override int SaveChanges() //TEK ÝSTEDÝÐÝM BOOKSTORDBCONTEXT ÜZERÝNDEN ERÝÞÝLEBÝLÝNÝR OLMASI BU YÜZDEN BÖYLE BÝR TAKLA ATTIRIYPORUZ BURADA (UNÝT)
        {
            return base.SaveChanges();   //base = DbContext
        }


    }
}


