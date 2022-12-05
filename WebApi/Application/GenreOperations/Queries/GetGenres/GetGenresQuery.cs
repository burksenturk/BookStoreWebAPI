using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WebApi.DbOperations;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Queries.GetGenres
{
    public class GetGenresQuery
    {
        public readonly IBookStoreDbContext _context;
        public readonly IMapper _mapper;
        public GetGenresQuery(IBookStoreDbContext context, IMapper mapper)   //store ve mapper ı alabilmemiz lazım constructor açtık
        {
            _context = context;
            _mapper = mapper;
        }

        public List<GenresViewModel> Handle()
        {
            //neden Where? isActive diye bir kolon yarattık dolasıyla o kolonu Isactive i true olan Genre ları bu Query ile getirmek istiyoruz
            var genres = _context.Genres.Where(x => x.IsActive).OrderBy(x => x.Id);  
            List<GenresViewModel> returnObj = _mapper.Map<List<GenresViewModel>>(genres);
            return returnObj;
        }

    }

    public class GenresViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    //bundan sonra mapping yapıcaz. mappingprofile da hangi modelden hangi modele iki obje arasında mapping yapacaksam tanıtıcaz.
}
