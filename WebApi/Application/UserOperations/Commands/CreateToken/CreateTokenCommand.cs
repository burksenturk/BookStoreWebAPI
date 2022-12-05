using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using WebApi.DbOperations;
using WebApi.DBOperations;
using WebApi.Entities;
using WebApi.TokenOperations;
using WebApi.TokenOperations.Models;

namespace WebApi.Application.UserOperations.Commands.CreateToken
{
    public class CreateTokenCommand
    {


        public CreateTokenModel Model { get; set; }

        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CreateTokenCommand(IBookStoreDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }


        public Token Handle() //createToken yaratan bir endpoint bu biizm için
        {
            //ilk bu user benim database imde var mı diye bakıyoruz..çümkü var olmayan bir user a bi token yaratamam
            var user = _dbContext.Users.FirstOrDefault(x=>x.Email == Model.Email && x.Password == Model.Password); // bana doğru bir mail ve paswordla mı gelmiş?
            if (user is not null)
            {

                TokenHandler handler = new TokenHandler(_configuration);
                Token token = handler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpireDate = token.Expiration.AddMinutes(5);
                _dbContext.SaveChanges();

                return token;

            }
            else
                throw new InvalidOperationException("Kullanıcı adı - şifre hatalı");

        }
    }

    public class CreateTokenModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
