using AutoMapper;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.Application.BookOperations.Queries.GetBooks;
using WebApi.Application.GenreOperations.Queries.GetGenreDetail;
using WebApi.Application.GenreOperations.Queries.GetGenres;
using WebApi.Entities;
using static WebApi.Application.BookOperations.Commands.CreateBook.CreateBookCommand;
using static WebApi.Application.UserOperations.Commands.CreateUser.CreateUserCommand;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    #region information
    //auotomapperın configlerinin buradan alacagını söylemek istiyorsak profile dan kalıtım aldırıyoruz 
    //obje dönüşümleinin konfigürsayonunu bir yerde verirsek ondan sonra her yerde o iki obje arasında mapper.map metoduyla dönüşüm yapabilirz.
    #endregion
    {
        public MappingProfile() //configleri vericez ne neye dönüşebilir onu ayarlıcaz
        {
            //ilk create endpointine mapper göndermeye çalısıcaz 
            CreateMap<CreateBookModel, Book>(); //CreateBookModel den Book model e maplenebilir olsun diyoruz.. ilk kısım source ikinci kısım target dır 

            /* Formember(her bi satır için) bunu uygula diyoruz. dest ' teki genre şu şekilde olusur diyorum..şu şekilde maple diyroum(opt) ... string karşılıgını genreye maple */
            CreateMap<Book,BookDetailViewModel>().ForMember(dest => dest.Genre , opt => opt.MapFrom(src => src.Genre.Name));
            //aşağıdaki ve yukarıdaki yukarıdaki mappingi Genre entitysi ekledikten sonra düzelttik..GEnre tablosundaki Name e denk geliyor bu diyoruz.

           CreateMap<Book,BooksViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));  //eski hali .MapFrom(src => ((GenreEnum)src.GenreId).ToString()));

            /*bunu(Genre) dönüştür olduğu gibi dönüştür diyorum*/
            CreateMap<Genre, GenresViewModel>();
            CreateMap<Genre, GenreDetailViewModel>();
            CreateMap<CreateUserModel, User>();




        }

    }
}
