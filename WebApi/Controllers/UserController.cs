
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using WebApi.Application.UserOperations.Commands.CreateToken;
using WebApi.Application.UserOperations.Commands.CreateUser;
using WebApi.Application.UserOperations.Commands.RefreshToken;
using WebApi.DbOperations;
using WebApi.DBOperations;
using WebApi.TokenOperations.Models;
using static WebApi.Application.BookOperations.Commands.CreateBook.CreateBookCommand;
using static WebApi.Application.UserOperations.Commands.CreateUser.CreateUserCommand;
using static WebApi.Applicationi.BookOperations.Commands.UpdateBook.UpdateBookCommand;

namespace WebApi.Controllers
{
    [ApiController] //controllerimızın http response döneceğini bu attiribute ile söylüyoruz
    [Route("[controller]s")]
    public class UserController : ControllerBase   //resource miz esasında bir sınıftır
    {
        private readonly IBookStoreDbContext _context;
        private readonly IMapper _mapper;
        readonly IConfiguration _configuration; //config bilgilerine ulasmamı sağlıyor..appsettings altındaki verilere ulasmamı sağlıyor

        public UserController(IBookStoreDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserModel newUser )
        {
            CreateUserCommand command = new CreateUserCommand(_context, _mapper);
            command.Model=newUser;
            command.Handle();

            return Ok();
        }

        [HttpPost("connect/token")] //Bir Token yaratma işlemi
        public ActionResult<Token> CreateToken([FromBody] CreateTokenModel  Login)
        {
            CreateTokenCommand command = new CreateTokenCommand(_context, _mapper, _configuration);
            command.Model = Login;
            var token = command.Handle();
            return token;
        }
        //token ımı refresh eden bir endpoint yaratıcam
        [HttpGet("refreshToken")] 
        public ActionResult<Token>RefreshToken([FromQuery] string token)  //refreshtoken ı bu requeste Query den göndericez query den okucaz
        {
            RefreshTokenCommand command = new RefreshTokenCommand(_context, _configuration);
            command.RefreshToken = token;
            var resultToken = command.Handle();
            return resultToken;
        }
    }
}
