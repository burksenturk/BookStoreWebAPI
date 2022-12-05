using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DBOperations;

namespace WebApi
//program.cs=DAtaGenerator
//BookController =BookStoreDbContext
//DAtaGenerator=BookStoreDbContext

//CreateHostBuilder() host oluþturuyor ayaða kaldýrýyor sunucuyu..configlerini .UseStartup ýný gidip
//<Startup>=þu classdan alýr diyoruz
{   //CreateHostBuilder() metodu startup cs in çalýsabilmesi için vardýr.<Startup> classýmýzýn bir config dosyasý oldugunu uygulamamýza söylüyor..  
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())     //uygulama ayaða kalktýgýnda verileri ekledik bunu bir kere yaparýz
            {                                                   
                var services = scope.ServiceProvider;
                DataGenerator.Initialize(services);      //program.cs i serviceprovider ile baðlantýya gecirdik
            }
                
                host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => 
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

/*    GENRE CONTROLLER EKLEME
 * 1) Entities oluþturup içinde grenres açtýk
    2) DataGeneratorda initialize olusturduk.
    3)bookstoredbcontext de genre baðlantýsý kurduk 
    4)BookOperations ve genreoperationslarý application altýnda topladýk.. kendi içlerinde de commands ve queries diye ayýrdýk.
    5)GEtgenres yazmaya baþladýk .. mapper falan ekledik.. handle ardýndan da view model oluþturduk
    6) mappingProfile da Genreviewmodelle Genre yi mapledik
7)handle() a geri döndük 
8)getdetail yazmaya baþladýk validasyon yaptýk 
9)bu comman ve queryleri yazdýktan sonra GenreControllerý hallettik
10)book entitysinde Genre yi foreign yaptýk daha sonra
11)bookdetailquery ve bookquery de incloud ettik Genre yi Include(x => x.Genre)
12)Genre enum uçurduk kurtulduk 



 
 
 */
