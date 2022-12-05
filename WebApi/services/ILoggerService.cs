namespace WebApi.services
{
    public interface ILoggerService 
    {
        public void Write(string message); //bu aslında Log yazma işlemi yapan metodumuzun adı olacak 

    }  //bundan implemente oalcak sınıfları yazıcaz ConsoleLogger ve DBlogger yazmayı planladık şu an için ikiside console a yazsada bi ifadeyle hangisinin çalsıtıgını görücez
}
