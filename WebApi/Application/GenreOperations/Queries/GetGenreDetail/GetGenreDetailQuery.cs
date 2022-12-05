using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Application.GenreOperations.Queries.GetGenres;
using WebApi.DbOperations;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Queries.GetGenreDetail
{
    public class GetGenreDetailQuery
    {
        public int GenreId { get; set; }
        public readonly IBookStoreDbContext _context;
        public readonly IMapper _mapper;
        public GetGenreDetailQuery(IBookStoreDbContext context, IMapper mapper)   //store ve mapper ı alabilmemiz lazım constructor açtık
        {
            _context = context;
            _mapper = mapper;
        }

        public GenreDetailViewModel Handle()
        {
            //neden Where? isActive diye bir kolon yarattık dolasıyla o kolonu Isactive i true olan Genre ları bu Query ile getirmek istiyoruz
            var genre = _context.Genres.SingleOrDefault(x => x.IsActive && x.Id == GenreId);
            if (genre == null)
                throw new InvalidOperationException("Kitap Türü Bulunamadı");

            return  _mapper.Map<GenreDetailViewModel>(genre);
            
        }
    }

    public class GenreDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
