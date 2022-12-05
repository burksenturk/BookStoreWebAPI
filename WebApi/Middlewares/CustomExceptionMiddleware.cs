using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebApi.services;

namespace WebApi.Middlewares
{
    public class CustomExceptionMiddleware
    {
        #region Ilogger service bilgisi
        //Middleware içerisinde DI container a inject edilen servisleri alabiliyoruz  
        #endregion
        private readonly ILoggerService _LoggerService;
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next, ILoggerService loggerService)
        {
            _next = next;
            _LoggerService = loggerService;
        }

        public async Task Invoke(HttpContext context)   
        {
            #region watch information
            //izlemeye başla bana bi tane watch oluştur diyoruz..bu requestten response a geçen süreyi öğrenmeye yarıyor.. watch.Elapsed.TotalMilliseconds burada kullandık
            #endregion
            var watch = Stopwatch.StartNew();
            try //burası başarılı gerçekleşme 
            { 
            #region Task information
            //loga yazacağım mesaja karar veriyorum request response arası.. + context.Request.Method + = gelen http metodumun tipini öğrendiğim yer
            //context.Request.Path=endpoint bilgisini gönder..
            #endregion
            string message = "[Request] HTTP" + context.Request.Method + " - " + context.Request.Path;
            _LoggerService.Write(message);

            #region await _next(context); ve watch.stop() information
                /* bu şekilde de bi sonraki middleware çağırılabiliyor  * //NOT2 bizim kırılmamız burda olabilir requestten sonra throw falan fırlartılabilir burada yakalamalıyız*/
                #endregion
            await _next(context);
            watch.Stop();
           
            message = "[Response] HTTP" + context.Request.Method + " - " + context.Request.Path + " responded " + context.Response.StatusCode + " in " + watch.Elapsed.TotalMilliseconds + " ms ";
            #region message bilgi
            /*burada bunun ne kadar sürede çalıştığını ve sonuçta bu http requestine karşılık  
             * hangi http post mesajısnın post statuscode nun döndüğünü log içerisinde göstermek istiyoruz ve geçen süreyi*/
            #endregion
            _LoggerService.Write(message);
            }
            catch(Exception ex)
            {
                watch.Stop(); //eğer hata alırsa yukarıda kırılma yerinde saatin düzgün çalışması için stopu burada yap dedik             
                #region HandleException information
                //exceptionlarımı mantıklı bir şekilde yazabilmek için bir metot yazdık..catch in çok büyümesini istemiyorum
                // watch verdik çünkü time ulaşıyor olmamız gerekiyor
                #endregion
                await HandleException(context, ex, watch); 
            }
        }
        #region Task information
        /*Task sınıfı herhangi bir asenkron operasyonun çalışma sürecindeki tüm bilgilerini tutan bir sınıftır. 
         Haliyle async ve await komutları Task tipinden faydalanarak gereken yapıları oluşturmakta ve sürecin asenkron bir şekilde ilerlemesini sağlamaktadırlar.*/
        #endregion
        private Task HandleException(HttpContext context, Exception ex, Stopwatch watch)   //yukarıyı generated ederek olustuduk  bu metodu.
        {
            #region aşağıdakilerle ilgi bilgi
            //context e yazarak geriye mesaj dönebilirim..eğer hata olduysa ben geriye bir şey dönmicem sadece mesaj dönücem demektir
            // "application/json"; ==içerik türü
            // InternalServerError = 500 olarak dönüyor
            #endregion
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //hata durumunda loga yazacagımız ve geriye döneceğimiz mesajı olusturucaz
            string message = "[Error] HTTP" + context.Request.Method + " - " + context.Response.StatusCode + " Error Message " + ex.Message + "in" + watch.Elapsed.TotalMilliseconds + "ms";
            _LoggerService.Write(message);  //cw ye elveda ettik bu geldi yerine
            #region Console.WriteLine(message); hata mesajı information
            /* buraya kadar hala ekrana hata mesajını dönemiyor anlamlı mesajı..bu noktada 200 ok dönsün istemiyoruz 500 server error dönsün istiyoruz
             NOT! DI da bunu değiştiricez beni rahatsız ediyor tek bi servisden yönetmek mantıklı..*/
            #endregion

            #region json dönüşümü,mesaj gösterimi vs bilgiler
            // **Exception objemin geriye bir json olarak dönmesini istiyorum çünkü bunun bir UI a döndüğünü varsayarsak ++
            // ben bunu UI içerisinde ne kadar json ile taşırsam response içersiinde UI tarafında çözümlenmesi kolay olur.
            // **Exception objesini(ex.message) seralize etmemiz lazım json a cevirmek için..bundan dolayo json paketi ekledik uygulama içerisine
            #endregion
            var result = JsonConvert.SerializeObject(new { error = ex.Message }, Formatting.None);
            return context.Response.WriteAsync(result);
            #region json dönüşümü,mesaj gösterimi vs bilgiler2
            //JsonConvert:newtonsoft.json dan geliyor.. Error için bir format belirledik..error için gerekli noktaları oluşturduk  
            // Console a basıyoruz[Console.WriteLine(message);] ondan sonra bu context içindeki respons u context.response objesini ezerek ++
            //exception durumlarında benim son kullanıcıma api üzerinden hangi mesajın döneceğini söylemiş olduk.
            #endregion

        }
    }
    public static class CustomExceptionMiddlewareExtension //hem loglama hem exception ı bi arada yapmayı planlıyoruz burada
    {
        public static IApplicationBuilder UseCustomExceptionMiddle(this IApplicationBuilder builder)  // startup.cs  de çağıracıgımız adı belirledik
        {
            return builder.UseMiddleware<CustomExceptionMiddleware>(); //<CustomExceptionMiddleware> burada hangi mw i kullanacagımızı söylüyoruz
        }
    }
}
