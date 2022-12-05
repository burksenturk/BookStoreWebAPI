using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;
using WebApi.DbOperations;
using WebApi.DBOperations;
using WebApi.Middlewares;
using WebApi.services;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }                                                            
                                                                                                                 

        #region ConfigureServices metodu bilgileri
        //ConfigureServices(IServiceCollection services) ile uygulalamam�z i�ersiinde kullanacag�m�z bile�enlerin ayarlar�n� veriyoruz
        //bu bile�enleri s��n�flar k�t�phaneler kod par�alar� gibi d���nebiliriz..
        //ConfigureServices(IServiceCollection services) bu metoda uygulaman�n kullanacag� servisler yani bile�enler eklenir..
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {                                                                            //token �m�z�n configlerini yapt�g�m�z token�m�z�n nas�l �al�sacag�n� belirtti�imiz yer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                //Token �n nas�l valide edilece�inin parametrelerini ver�iyorum burada.
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true, //audience �u benim bu token�m� kimler kullanabilir,audienceclient �m kimlerdir..bunlar� valide et diyorum �u an kural� veriyoruz sadece audienceyi s�ylemedik
                    ValidateIssuer = true, //bu token �n da��t�s�c�s� sa�lay�c�s� kim bununda vali�dasyonunu yap diyorum
                    ValidateLifetime = true, //mutlaka lifetime � kontrol et. lifetime tamamland�ysa token expire olsun ve yetkilendirmeyi kapat eri�ilemez olsun
                    ValidateIssuerSigningKey = true, //token� imzal�yacag�m�z kriptol�yacag�m�z anahtar key bunuda kontrol et dedik
                    ValidIssuer = Configuration["Token: Issuer "], //bu token �n�n yarat�l�rken ki �ssuer u  �unlard�r
                    ValidAudience = Configuration["Token: Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
                    #region �ssuerSigninKey
                    //bundan t�retiliyor ve encod ediliyor.. nas� lemcod etti UTF8... orada bi security key tutucam GetBytes(Configuration[]) onu da �ifreleyip
                    //SymmetricSecurityKey olusturup onu da  IssuerSigningKey olarak kullan�cam.
                    #endregion
                    ClockSkew = TimeSpan.Zero
                    #region clockSkew
                    //token � �reten sunucumuzun var oldugu timezone ile token � sa�lad�g�m�z kullan�cak olan  clientlerin timezone u birbirinden farkl� oldugu durumda �rne�in ba�ka yerde
                    //+3 saat daha ileide oldugunu d���n�rsek dolas�yla token �n�n orada erken sonlanmas� gibi bi udurum s�z konusu olacak.ve token �n�n t�m zone larda adil bir �ekilde da��t�lanilmesi i�in 
                    //clockskew diye bir parametre var..diyorum ki Token �n�n expiration date nin �zerine �u kadar(ClockSkew=TimeSpan.Zero) koy diyorum bu da onun �zerinde koyacag�m�z time � belirtiyor.
                    #endregion


                };
            });

            services.AddControllers();     
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });    //SwaggerUI implemente edilmi�
            });
            //database implemente ediyoruz ��nk� db de bir servisdir..uygulama i�reisinde kullanabilmemiz i�in burada inject etmeliyiz
            services.AddDbContext<BookStoreDbContext>(options => options.UseInMemoryDatabase(databaseName: "BookStoreDB"));
            #region bilgiler
            //Bu �ekilde yaparak ben art�k uygulamama contextimi g�stermi� oldum..
            //servislere git DbContext olarak bunu=> <BookStoreDbContext> ekle dedik
            #endregion
            services.AddScoped<IBookStoreDbContext>(provider => provider.GetService<BookStoreDbContext>()); //UN�T
            #region bilgiler
            //Scope:inject edilen servisin request lifetime i�erisinde ya��yor olmas�.yani bir request geldi�inde bir instance olusturuluyor b� Ibookstoredbcontext in bir nesnesi olusuyor
            //ve o requeste istinaden bir response d�nene kadar uygulama i�erisinde ya�s�yor.bitti�i anda o response d�nd��� anda yok ediliyor yeni request geldi�inde
            //yeni bir bookstoredbcontext olusturuyor.
            #endregion

            services.AddAutoMapper(Assembly.GetExecutingAssembly()); //automapper implemente etmi� olduk
            services.AddSingleton<ILoggerService, ConsoleLogger>(); // <hangi s�n�ftan interface ald�g�n�,hangi s�n�f�n �al��acag� DBLogger yazarsan orada yazar>
        }

        #region Configure metodu bilgileri
        //uygulamam�za gelen http isteklerinin hangi a�amalardan gecerek bir http metodu olu�turacag�m�z� belirtti�imiz metottur
        //Startup i�erisinde en ba�ta bu metodun do�ru ayarlanmas� laz�m
        //MiddleWare arakatman yaz�l�mlar� ile pipeline olusturyoruz configure() i�erisinde de bu pipline n�n ayarlar�n� veriyoruz
        #endregion
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));  // development ortamn�nda SwaggerUI implemete edilmi�
            } 
            app.UseAuthentication();  //BU GELD������

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomExceptionMiddle();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

//