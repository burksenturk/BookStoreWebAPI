 using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.DbOperations;
using System.Linq;
using WebApi.Common;
using System;
using AutoMapper;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Queries.GetBookDetail
{ 
    public class GetBookDetailQuery
    {
        //bir Id ye ihtiyacımız var..hangi Id li kitabın detayını istiyorsak onu burada tanımlıcaz ki dışarıdan set edilebilsin..
        public int BookId { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetBookDetailQuery(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BookDetailViewModel Handle()
        {
            //aşağıdaki book u viewmodel e Maplememiz lazım
            var book = _dbContext.Books.Include(x=>x.Genre).Where(book => book.Id == BookId).SingleOrDefault();  //Include(x=>x.Genre) bunu ekledik genre foreign keyinden sonra
            if (book is null)
                throw new InvalidOperationException("Kitap Bulunamadı!");

            BookDetailViewModel vm = _mapper.Map<BookDetailViewModel>(book); /* book datası verisini BookDetailViewModel e MAPLAEDİK... */
            return vm;
        }

        //geriye BookDEtail i kontrollü bir şekilde dönmek istiyorum Book ENtitysinin dönmek istemiyorum bunun yanlış olduğunu bildiğim için 

    }

    public class BookDetailViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }  //buradaki Genre yi artık kendi entity imden getirmek istitorum burayı genre entity imize bakcak şekilde değiştircez.. aslında Genre Enumdan komple kurtulucaz
      //string Genre yi yukarıdaki Name Genre ye mapliyoruz mappingprofile da
        public string PublishDate { get; set; }
        public int PageCount { get; set; }

    }

}