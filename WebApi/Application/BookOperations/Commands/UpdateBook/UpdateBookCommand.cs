using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Reflection.Metadata;
using WebApi.DbOperations;
using WebApi.DBOperations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApi.Applicationi.BookOperations.Commands.UpdateBook
{
    
    public class UpdateBookCommand
    {
        public int BookId { get; set; }

        private readonly IBookStoreDbContext _dbContext;

        //updateBookModel() dışarıdan parametre olarak alınması lazım ondan aşağıdakini yapıyoruz
        public UpdateBookModel Model { get; set; }
        public UpdateBookCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {

            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);

            if (book is null)
                throw new InvalidOperationException("güncellenecek kitap bulunamadı");

            book.Title = Model.Title != default ? Model.Title : book.Title; //  UpdateBook.Title defaul değilse(?) UpdateBook.Title  bunu kullan eğer(:) default ise book.Title kullan..
            book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;

            _dbContext.SaveChanges();
            
        }

        public class UpdateBookModel   //publishDAte ve pagecount update edilmesin isteedik
        {
            public string Title { get; set; }
            public int GenreId { get; set; }
        }

    }

    //model update içi çok daha anlamlı çünkü bir entity içerisindeki her field ı update ettirmeyebiliriz..
   // modellerle neleri update ettirmek istiyorsak onun için model yaratıyor olmak çok daha anlamlı


}
