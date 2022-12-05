using AutoMapper;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.DbOperations;
using WebApi.Entities;
using WebApi.UnitTests.TestSetup;
using Xunit;
using static WebApi.Application.BookOperations.Commands.CreateBook.CreateBookCommand;

namespace WebApi.UnitTests.Application.BookOperations.Commands.CreateBook
{
    //bu ısınıfa textfiture ı tanıtmak lazım..ben bu sınıfa config veriyorum bu sayede
    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTests( CommonTestFixture testFixture) //bu fixture ın bir instance sini aldım burada  dolayısıyla  TestFixture constructure ı çalıstı++
            //++ ve constructure ı sonucunda yapılan configleri ben bu createbookcommand constructure mın içerisinden erşebilir durudmayım.
        {
            _context=testFixture.Context; //sen testFixture dan gelen Context i kullan dedik.
            _mapper=testFixture.Mapper; //bu durumda _context e ve _mapper a erişebilir durumda olurum.
        }

        //metodun bir test metodu oldugunu çalısıtırılacak testler arasına girmesi gerektiğini söylicez FACT ile
        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            //arrange(hazırlık)
            var book = new Book() { Title = "Test_WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn", PageCount = 100, PublishDate = new System.DateTime(1990, 01, 10), GenreId = 1 };
            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper); //ikisinide konfigure ediyoruz 
            command.Model = new CreateBookModel() { Title = book.Title }; // Modeli Set ettik... Title ı db ye yazzmıs oldugum book.Title göndericem ki bana kızsın zaten var desin.
            //act & assert (Çalıştırma - Doğrulama)
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("kitap zaten mevcut");
        }

        [Fact]   //happy path testi
        public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            CreateBookModel model = new CreateBookModel() { Title = "Hobbit",PageCount = 100, PublishDate = DateTime.Now.Date.AddYears(-10),GenreId=1};
            command.Model = model;

            //act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            //asserts
            var book = _context.Books.SingleOrDefault(book => book.Title == model.Title);

            book.Should().NotBeNull();
            book.PageCount.Should().Be(model.PageCount);
            book.PublishDate.Should().Be(model.PublishDate);
            book.GenreId.Should().Be(model.GenreId);
        }
    }
}
