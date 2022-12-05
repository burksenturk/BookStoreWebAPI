using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Entities
{
    public class Book   //book entitysi gibi d���n..entity olusturunca bunu u�urucaz muhtemelen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Auto Increment ID kolonunun eklenmesini sa�layan attiribute entityframework �n �zelli�i
        public int Id { get; set; }

        public string Title { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }  //int GenreId buradaki Genre Entitysi ile foreign key ili�kisi vard�r.
        //book entitysi ile Genre Entitysi aras�nda relation bir ili�ki olmu� oldu..buradaki Genre�d Genre tablosundaki Id ye refere ediyor ba�lant�y� kurdu 
        public int PageCount { get; set; }

        public DateTime PublishDate { get; set; }
    }
}


