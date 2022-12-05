using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApi.DbOperations;
using System.Linq;
using WebApi.Common;
using AutoMapper;
using System;
using WebApi.Entities;
using WebApi.DBOperations;

namespace WebApi.Application.BookOperations.Queries.GetBooks
{
    public class GetBooksQuery
    {
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public GetBooksQuery(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        //asıl işi yapacak olan metodumuzhandle.. Bir stanfart oluşturmaya çalısıyoruz

        public List<BooksViewModel> Handle()  
        {
                                                    //x.Genre entitysini bana include et dedim çünkü ben bunu mapping yapmak için kullanıcam..o veriye ihtiyacım var o tabloyla join yapmaya ihtiyacım var 
            var bookList = _dbContext.Books.Include(x => x.Genre).OrderBy(x => x.Id).ToList<Book>();  // orderby la sıraladık Id ye göre, enumereybıl oldugu için Tolist yapıp geriye list döndük
            List<BooksViewModel> vm = _mapper.Map<List<BooksViewModel>>(bookList);   //resource booklist ... target(hedef) List<BooksViewModel> budur. rosurce tagete maplanir.

            return vm;  

        }

    }

    // UI a döneceğimiz veri setini ViewModel İle korumak istiyoruz.isteğdiğim veri tipinin Uİ a döndüğünden her zaaman için emin olmak istiyorum.
    //BUNUN İÇİN VİEWMODEL YAPACAZ 

    public class BooksViewModel   //burada olusturduk çünkü dışarıdan çok bilinmesine gerek yok..
    {
        
        public string Title { get; set; }
        public int PageCount { get; set; }

        public string PublishDate { get; set; }

        public string Genre { get; set; }  //uı ya göstereceğim için GenreId mantıksız oluyor..buraya logic koymus olduk yani yapılandırma.. viewmodelde yaparız böyle şeyler
    }

}





//KODUMUZU REFACTOR EDEREK DAHA TEMİZ VE GENİŞLEYEBİLİR BİR HALE GETİRİCEZ..

//BOOKCONTROLLERİN ÇOK BÜYÜDÜĞÜNÜ DÜŞÜN ORADA ÇOK FAZLA LOGİC OLACAK VE OKUNABİLİRLİĞİ DÜŞÜK BİR HALE GELECEK (Aynı işi yapan metotları kodları ayrıştırarak
//her birini ayrı classlarda metotlarda yönetmek daha doğrudur) o yüzden bookcontrollerdaki metotları ayrı ayrı sınıflara alıp parçaladık daha sonrada dönüş değerlerini
//viewmodele çeviricez get de update falan bir model kullanıcaz kontrol altına alabilmek için.


//inpur olarak gelen modelleri işte entityleriiviewmodelleri,dto ları mümkün oldugunca birbirinden ayırmak gerekiyor çünku bunu yapmazsak dependency yaratırız.. 