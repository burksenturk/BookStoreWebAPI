using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class Book   //book entitysi gibi düþün..entity olusturunca bunu uçurucaz muhtemelen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Auto Increment ID kolonunun eklenmesini saðlayan attiribute entityframework ün özelliði
        public int Id { get; set; }

        public string Title { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }  //int GenreId buradaki Genre Entitysi ile foreign key iliþkisi vardýr.
        //book entitysi ile Genre Entitysi arasýnda relation bir iliþki olmuþ oldu..buradaki Genreýd Genre tablosundaki Id ye refere ediyor baðlantýyý kurdu 
        public int PageCount { get; set; }

        public DateTime PublishDate { get; set; }
    }
}


