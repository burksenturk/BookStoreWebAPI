using System;
using System.Linq;
using WebApi.DbOperations;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.Application.GenreOperations.Commands.CreateGenre
{
    public class CreateGenreCommand
    {
        //dışlarıdan vereceğimiz modeli oluşturduk.
        public CreateGenreModel Model { get; set; }
        private readonly IBookStoreDbContext _context;

        public CreateGenreCommand(IBookStoreDbContext context)  //burada injection yapıyporuz ama DI container la değil aslında biz instance sini direkt kendimiz veriyoruz bu da injection çünkü direkt bağımlılığı ortadan kaldırmış oluyoruz. 
        {
            _context = context;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Name == Model.Name);
            if( genre is not null)
                throw new InvalidOperationException("Kitap Türü Zaten Mevcut!");

            //burada mapleme yapmak istemiyoruz zaten iki tane fieldım var direkt gelen veriden kendimiz yaratıcaz
            //Genre() entityden geldi.
            genre = new Genre(); //boş bir genre instancasi yarattık şimdi onu setlicez
            genre.Name = Model.Name;
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
    }

    public class CreateGenreModel
    {
        public string Name { get; set; }  //ilk olusturulmada isActive true oldugundan ve id autoincrement oldugundan sadece name yarattık
    }
}
