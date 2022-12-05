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
        //ConfigureServices(IServiceCollection services) ile uygulalamamýz içersiinde kullanacagýmýz bileþenlerin ayarlarýný veriyoruz
        //bu bileþenleri sýýnýflar kütüphaneler kod parçalarý gibi düþünebiliriz..
        //ConfigureServices(IServiceCollection services) bu metoda uygulamanýn kullanacagý servisler yani bileþenler eklenir..
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {                                                                            //token ýmýzýn configlerini yaptýgýmýz tokenýmýzýn nasýl çalýsacagýný belirttiðimiz yer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                //Token ýn nasýl valide edileceðinin parametrelerini verþiyorum burada.
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true, //audience þu benim bu tokenýmý kimler kullanabilir,audienceclient ým kimlerdir..bunlarý valide et diyorum þu an kuralý veriyoruz sadece audienceyi söylemedik
                    ValidateIssuer = true, //bu token ýn daðýtýsýcýsý saðlayýcýsý kim bununda valiþdasyonunu yap diyorum
                    ValidateLifetime = true, //mutlaka lifetime ý kontrol et. lifetime tamamlandýysa token expire olsun ve yetkilendirmeyi kapat eriþilemez olsun
                    ValidateIssuerSigningKey = true, //tokený imzalýyacagýmýz kriptolýyacagýmýz anahtar key bunuda kontrol et dedik
                    ValidIssuer = Configuration["Token: Issuer "], //bu token ýnýn yaratýlýrken ki ýssuer u  þunlardýr
                    ValidAudience = Configuration["Token: Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
                    #region ýssuerSigninKey
                    //bundan türetiliyor ve encod ediliyor.. nasý lemcod etti UTF8... orada bi security key tutucam GetBytes(Configuration[]) onu da þifreleyip
                    //SymmetricSecurityKey olusturup onu da  IssuerSigningKey olarak kullanýcam.
                    #endregion
                    ClockSkew = TimeSpan.Zero
                    #region clockSkew
                    //token ý üreten sunucumuzun var oldugu timezone ile token ý saðladýgýmýz kullanýcak olan  clientlerin timezone u birbirinden farklý oldugu durumda örneðin baþka yerde
                    //+3 saat daha ileide oldugunu düþünürsek dolasýyla token ýnýn orada erken sonlanmasý gibi bi udurum söz konusu olacak.ve token ýnýn tüm zone larda adil bir þekilde daðýtýlanilmesi için 
                    //clockskew diye bir parametre var..diyorum ki Token ýnýn expiration date nin üzerine þu kadar(ClockSkew=TimeSpan.Zero) koy diyorum bu da onun üzerinde koyacagýmýz time ý belirtiyor.
                    #endregion


                };
            });

            services.AddControllers();     
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });    //SwaggerUI implemente edilmiþ
            });
            //database implemente ediyoruz çünkü db de bir servisdir..uygulama içreisinde kullanabilmemiz için burada inject etmeliyiz
            services.AddDbContext<BookStoreDbContext>(options => options.UseInMemoryDatabase(databaseName: "BookStoreDB"));
            #region bilgiler
            //Bu þekilde yaparak ben artýk uygulamama contextimi göstermiþ oldum..
            //servislere git DbContext olarak bunu=> <BookStoreDbContext> ekle dedik
            #endregion
            services.AddScoped<IBookStoreDbContext>(provider => provider.GetService<BookStoreDbContext>()); //UNÝT
            #region bilgiler
            //Scope:inject edilen servisin request lifetime içerisinde yaþýyor olmasý.yani bir request geldiðinde bir instance olusturuluyor bý Ibookstoredbcontext in bir nesnesi olusuyor
            //ve o requeste istinaden bir response dönene kadar uygulama içerisinde yaýsýyor.bittiði anda o response döndüðü anda yok ediliyor yeni request geldiðinde
            //yeni bir bookstoredbcontext olusturuyor.
            #endregion

            services.AddAutoMapper(Assembly.GetExecutingAssembly()); //automapper implemente etmiþ olduk
            services.AddSingleton<ILoggerService, ConsoleLogger>(); // <hangi sýnýftan interface aldýgýný,hangi sýnýfýn çalýþacagý DBLogger yazarsan orada yazar>
        }

        #region Configure metodu bilgileri
        //uygulamamýza gelen http isteklerinin hangi aþamalardan gecerek bir http metodu oluþturacagýmýzý belirttiðimiz metottur
        //Startup içerisinde en baþta bu metodun doðru ayarlanmasý lazým
        //MiddleWare arakatman yazýlýmlarý ile pipeline olusturyoruz configure() içerisinde de bu pipline nýn ayarlarýný veriyoruz
        #endregion
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));  // development ortamnýnda SwaggerUI implemete edilmiþ
            } 
            app.UseAuthentication();  //BU GELDÝÝÝÝÝÝ

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