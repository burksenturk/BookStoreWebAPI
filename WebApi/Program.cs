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

//CreateHostBuilder() host olu�turuyor aya�a kald�r�yor sunucuyu..configlerini .UseStartup �n� gidip
//<Startup>=�u classdan al�r diyoruz
{   //CreateHostBuilder() metodu startup cs in �al�sabilmesi i�in vard�r.<Startup> class�m�z�n bir config dosyas� oldugunu uygulamam�za s�yl�yor..  
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())     //uygulama aya�a kalkt�g�nda verileri ekledik bunu bir kere yapar�z
            {                                                   
                var services = scope.ServiceProvider;
                DataGenerator.Initialize(services);      //program.cs i serviceprovider ile ba�lant�ya gecirdik
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
 * 1) Entities olu�turup i�inde grenres a�t�k
    2) DataGeneratorda initialize olusturduk.
    3)bookstoredbcontext de genre ba�lant�s� kurduk 
    4)BookOperations ve genreoperationslar� application alt�nda toplad�k.. kendi i�lerinde de commands ve queries diye ay�rd�k.
    5)GEtgenres yazmaya ba�lad�k .. mapper falan ekledik.. handle ard�ndan da view model olu�turduk
    6) mappingProfile da Genreviewmodelle Genre yi mapledik
7)handle() a geri d�nd�k 
8)getdetail yazmaya ba�lad�k validasyon yapt�k 
9)bu comman ve queryleri yazd�ktan sonra GenreController� hallettik
10)book entitysinde Genre yi foreign yapt�k daha sonra
11)bookdetailquery ve bookquery de incloud ettik Genre yi Include(x => x.Genre)
12)Genre enum u�urduk kurtulduk 



 
 
 */
