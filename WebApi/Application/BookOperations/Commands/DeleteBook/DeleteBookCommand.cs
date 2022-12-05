using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;
using System.Reflection.Metadata;
using WebApi.DbOperations;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Commands.DeleteBook
{
    public class DeleteBookCommand
    {
        public int BookId { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        public DeleteBookCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
            if (book is null)
                throw new InvalidOperationException("silinecek kitap bulunamadı");


            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }
    }
}
