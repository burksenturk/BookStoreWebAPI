 using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using WebApi.DbOperations;
using WebApi.DBOperations;
using WebApi.Entities;

namespace WebApi.Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommand
    {
        //burada bir tane modele ihtiyacımız var çünkü bu modeli dışarıdan SETlicez çünkü kullanıcıdan geliyor.geldiği noktada da controller içerisinde de 
        //CreateBookCommand içerisinde bizim bir modeli SETlememiz lazımki buraya dolu bir şekilde gelsin.

        public CreateBookModel Model { get; set; }      

        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public CreateBookCommand(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }   


        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Title == Model.Title);   
            if (book is not null)
                throw new InvalidOperationException("kitap zaten mevcut"); 
            book = _mapper.Map<Book>(Model);  //MODEL İLE GELEN VERİYİ bOOK ObJESİ İÇERSİİNE O vERİLERİ MAPLE DEMEK

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

        }

        public class CreateBookModel //entity ile modeli ayırmak için açtık sonucta viewmodel sadece uı döner
        {
            public string Title { get; set; }   //bu noktada şunu düşünücez kitap yaratmak istediğimizde dışarıdan neleri almamız gerekiyor ++
            public int PageCount { get; set; }   //neleri kod içerisinde karar vermemiz gerekiyor
            public DateTime PublishDate { get; set; }
            public int GenreId { get; set; }  //string değil int almamız lazım çünkü value sine ihtiyacımız var 
        }
    }
}
