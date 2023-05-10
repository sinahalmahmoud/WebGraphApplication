using Microsoft.Graph;
using System.Drawing;


namespace WebGraphApplication
{
    public class Photos
    {
        public string userId { get; set; }
        public string PhotoURL { get; set; }
        public Photos(string userid, string photourl)
        {
            userId = userid;
            PhotoURL = photourl;
        }
        public Photos() { }
    }
  
}
