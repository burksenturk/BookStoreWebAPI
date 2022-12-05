using System;

namespace WebApi.TokenOperations.Models
{
    public class Token
    {
        #region Tokenoperations bilgi
        //token yaratma konusunda.. başka yerlerden de çağırılabilsin istiyorum..bu yüzden ayrı bir folder altında dboperations gibi tokenoperations açtık.
        #endregion

        //bu şu oluyor connectToken endpaointi. connectToken endpointi çağırıldıgında geriye gerşye dönülecek olan verileri tutan obje olacak bu..
        //bu da şu demek geriye bir Access token dönecek bir expiretiontime dönecek bir de refresh token dönecek ki bi sonraki çağrılarda o refresh token 
        //kullanılarak access token yaratılabilsin. 

            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public DateTime Expiration { get; set; }




    }
}
