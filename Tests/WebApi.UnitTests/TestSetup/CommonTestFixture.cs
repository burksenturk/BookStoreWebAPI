using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.DbOperations;

namespace WebApi.UnitTests.TestSetup
{
    public class CommonTestFixture
    {
        //burası bana contextle mapper ı verecek sınıf olacak..bu ikisini moq lıcaz işlemlerimizi yapabilmek için sahtesini olusturmak gibi
        //configler yapıyoruz.. inmemory database kurucaz 

        public BookStoreDbContext Context { get; set; }
        public IMapper Mapper { get; set; }

        public CommonTestFixture() //bunun bir nesnesi olustugunda constructuresi çalıstıgında ++
        {
            //  ++ilk önce db contexti yaratıyor olmamız gerekiyor ve context in optionslarını burada belirliyoruz
            var options = new DbContextOptionsBuilder<BookStoreDbContext>().UseInMemoryDatabase(databaseName: "BookStoreTestDB").Options;
            Context = new BookStoreDbContext(options); //bu COntext i şu şekilde yarat diyoruz..(options) bu da senin option nın.. böylece napmıs olduk tek bir Bookstoredbcontext objem vardı onun options ını test projem içinden değiştirip yaratmıs oldum
            Context.Database.EnsureCreated(); //bu context in yaratıldıgından emin olmak için bu metodu uyguluyoruz.
            Context.AddBooks();
            Context.AddGenres();
            Context.SaveChanges();

            ////configlerini orjinainden al dedik orada bi değişiklik oldugunda bTEst Mapper patlamasın
            Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); }).CreateMapper(); 
        }
    }
}
