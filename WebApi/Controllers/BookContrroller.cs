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
    //Bookcontroller�m� korumak istiyorum[Authorize] token �rettikten sonra 
    [Authorize]
    [ApiController] //controllerim�z�n http response d�nece�ini bu attiribute ile s�yl�yoruz
    [Route("[controller]s")]
    public class BookController : ControllerBase   //resource miz esas�nda bir s�n�ft�r
    {
        private readonly IBookStoreDbContext _context;  //bunu direkt incekt   (BookStoreDbContext=>IBookStoreDbContext  unit test a�amas�nda oldu butun s�n�flarda)
        #region BookStoreDbContext _context bilgiler
        //sadece burada kullanaca��m�z instance s�n� olusturduk..
        //_context i  BookStoreDbContext classda public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) deki options yerine at�p hareket verdin gibi d���n
        //readonly de�i�kenelr uyg. i�erisinde de�i�tirilemezler sadece constructor arac�l�g� ile set edilebilirler
        #endregion
        private readonly IMapper _mapper; 

        public BookController(IBookStoreDbContext context, IMapper mapper) //constructorlar arac�l�g� ile inject edilen DbContext i alabiliriz.
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()  //http200 bilgisi ve obje ile beraber return etti�imizde IActionResult geri d�n�� tipini kullan�r�z.
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
                query.BookId = id;//route dan ald�g�m id ye e�it BookId
                GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
                validator.ValidateAndThrow(query);
               result= query.Handle();


            
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook) //art�k d��ar�dan CreateBookModel al�yoruz
        {
            CreateBookCommand command = new CreateBookCommand(_context,_mapper);  //_MAPPER GELD� PARAMETREYE 
           
            
                 command.Model = newBook;   
                 CreateBookCommandValidator validator = new CreateBookCommandValidator();
                 //geriye bir �ey d�nme dedik..bu metot ise valide et hatay� throw et demek.
                 //burada hata al�rsak art�k exception katman�m�z geriye 500 error d�necek ve i�erisinde a��klamalar olacak(Middleware exception)
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
                //bookId be Model SETlendikten sonra fluentvalidation i�lemlerine ge�tik..
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
