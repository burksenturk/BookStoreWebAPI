using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.Application.BookOperations.Commands.DeleteBook;
using WebApi.Application.BookOperations.Queries.GetBookDetail;
using WebApi.Application.BookOperations.Queries.GetBooks;
using WebApi.Applicationi.BookOperations.Commands.UpdateBook;

using WebApi.Common;
using WebApi.DbOperations;
using WebApi.DBOperations;
using static WebApi.Application.BookOperations.Commands.CreateBook.CreateBookCommand;
using static WebApi.Applicationi.BookOperations.Commands.UpdateBook.UpdateBookCommand;

namespace WebApi.Controllers
{
    //Bookcontrollerýmý korumak istiyorum[Authorize] token ürettikten sonra 
    [Authorize]
    [ApiController] //controllerimýzýn http response döneceðini bu attiribute ile söylüyoruz
    [Route("[controller]s")]
    public class BookController : ControllerBase   //resource miz esasýnda bir sýnýftýr
    {
        private readonly IBookStoreDbContext _context;  //bunu direkt incekt   (BookStoreDbContext=>IBookStoreDbContext  unit test aþamasýnda oldu butun sýnýflarda)
        #region BookStoreDbContext _context bilgiler
        //sadece burada kullanacaðýmýz instance sýný olusturduk..
        //_context i  BookStoreDbContext classda public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) deki options yerine atýp hareket verdin gibi düþün
        //readonly deðiþkenelr uyg. içerisinde deðiþtirilemezler sadece constructor aracýlýgý ile set edilebilirler
        #endregion
        private readonly IMapper _mapper; 

        public BookController(IBookStoreDbContext context, IMapper mapper) //constructorlar aracýlýgý ile inject edilen DbContext i alabiliriz.
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()  //http200 bilgisi ve obje ile beraber return ettiðimizde IActionResult geri dönüþ tipini kullanýrýz.
        {
            GetBooksQuery query = new GetBooksQuery(_context,_mapper);
            var result = query.Handle();
            return Ok(result);  
        }

        [HttpGet("{id}")]
        public IActionResult GetById( int id)
        {
            BookDetailViewModel result;

                GetBookDetailQuery query = new GetBookDetailQuery(_context,_mapper);
                query.BookId = id;//route dan aldýgým id ye eþit BookId
                GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
                validator.ValidateAndThrow(query);
               result= query.Handle();


            
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook) //artýk dýþarýdan CreateBookModel alýyoruz
        {
            CreateBookCommand command = new CreateBookCommand(_context,_mapper);  //_MAPPER GELDÝ PARAMETREYE 
           
            
                 command.Model = newBook;   
                 CreateBookCommandValidator validator = new CreateBookCommandValidator();
                 //geriye bir þey dönme dedik..bu metot ise valide et hatayý throw et demek.
                 //burada hata alýrsak artýk exception katmanýmýz geriye 500 error dönecek ve içerisinde açýklamalar olacak(Middleware exception)
                 validator.ValidateAndThrow(command);
                 command.Handle();
                 return Ok();


        }

        [HttpPut("{id}")]   //bunlar endpointlerimiz verblerimiz

        public IActionResult UpdateBook(int id , [FromBody] UpdateBookModel UpdateBook)  //iki parametreli
        {

                UpdateBookCommand commands = new UpdateBookCommand(_context);
                commands.BookId = id;
                commands.Model = UpdateBook;
                //bookId be Model SETlendikten sonra fluentvalidation iþlemlerine geçtik..
                UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
                validator.ValidateAndThrow(commands);
                commands.Handle();


                return Ok();

        }

        [HttpDelete("{id}")]
        
        public IActionResult DeleteBook(int id)
        {

                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id;
                DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
                validator.ValidateAndThrow(command);
                command.Handle();

            return Ok();
        }

        





    }

}
