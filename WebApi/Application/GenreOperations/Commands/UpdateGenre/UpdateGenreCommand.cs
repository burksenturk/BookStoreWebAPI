using System;
using System.Linq;
using WebApi.DbOperations;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Commands.CreateGenre
{
    public class UpdateGenreCommand
    {
        public int GenreId { get; set; }
        public UpdateGenreModel Model { get; set; }
        private readonly IBookStoreDbContext _context;
        public UpdateGenreCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Id == GenreId);
            if (genre == null)
                throw new InvalidOperationException("Kitap Türü Bulunamadı!");
            #region bilgi
            //bakamaız gereken bir nokta daha var: bana dışarıdan bende ıd ye göre olmayan bi tane kitabı update etmek için geldi ama bana göndermiş oldugu isim halihazırda
            //başka bir kitap türünün adıysa yani bu ıd ye ait kitabın adı değilde başka bir ıd ye ait türde bu isim varsa benim hata evrmem gerekiyor.
            //ÖR: eğer 2 numaralı id li türün adı romance ve bana 1 numaralı id ve romance ile gelirse case i aslında bu bunu kontrol ettik
            //any() içinde bir tane kosulu sağlayan değer olursa true döner
            #endregion
            if (_context.Genres.Any(x => x.Name.ToLower() == Model.Name.ToLower() && x.Id != GenreId))
                throw new InvalidOperationException("Aynı isimli bir kitap türü zaten mevcut");

            genre.Name = String.IsNullOrEmpty(Model.Name.Trim()) ? genre.Name : Model.Name;   //video 45.dk
            #region bilgi
            /*string name in dafoultu*/
            //yapmaya calıstıgımız : dışarıdan bu genre nın id si ile bana gelip sadece isactive liğini değiştirebilsin bunun için bana tekrar name yi de göndermek zorunda kalmasın
            //eğer Name i string empty string gelirse ben buna kızmıyorum model.name i update etmicem modelden gelen name ye göre çünkü o zaman ismini null lamış oluruz
            //string empty yapmıs oluruz. o noktada kendi name i ile tekrar kendini update edicem ve isactive ile devam edicem.
            #endregion
            genre.IsActive = Model.IsActive;
            _context.SaveChanges();

        }
    }

    public class UpdateGenreModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;

    }
}

