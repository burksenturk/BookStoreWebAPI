using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Entities;
using WebApi.TokenOperations.Models;

namespace WebApi.TokenOperations
{
    public class TokenHandler
    {
        //hem acces hemde refresh token yaratacak olan bi sınıf olusturduk

        //burada Iconfigirationa ihtiyaacım olacak çünkü appsettingsten config okuyacagız.. o configlere gömre burada config olusturacazki  çözerkende o configlere bağlı olarak kolayca çözülebilsin
        public IConfiguration Configuration { get; set; }
        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //geriye token modelini dönen bi tane metot yapıcam
         public Token CreateAccessToken(User user) //user a göre geriye Token dönen bir metot bu
        {
            Token tokenModel = new Token();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));  //simetrik keyimizi olusturduk
            //şifrelenmiş bir kimlik olusuturuyoruz
            SigningCredentials credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);  //key ve algoritma verdik

            tokenModel.Expiration = DateTime.Now.AddMinutes(15);  //15 dk lık access token yaratmış olduk.

            //tokenın tipidir aslında bu
            //token ın ayarlarını olusturmus olduk.benim token ımın olusturulma şekli bu dedim.
            JwtSecurityToken securityToken = new JwtSecurityToken(   
                issuer: Configuration["Token:Issuer"],
                audience: Configuration["TokenAudience"],
                expires: tokenModel.Expiration,
                notBefore:DateTime.Now,  //ne zaman çalısmaya başlasın 15 dk sonra mı şimdi
                signingCredentials : credentials
                );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            tokenModel.AccessToken = tokenHandler.WriteToken(securityToken); //bu aşamada Toklen üretilmiş oluyor 
            tokenModel.RefreshToken = CreateRefreshToken();

            return tokenModel;
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();   
        }

    }
}
