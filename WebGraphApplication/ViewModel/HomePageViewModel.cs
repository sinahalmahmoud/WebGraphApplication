using Microsoft.Graph;

namespace WebGraphApplication.ViewModel
{
    public class HomePageViewModel
    {
        //TeamsApiResult
        public IList<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        public IList<string> counts { get; set; } = new List<string>();
        public IList<Photos> photos { get; set; }= new List<Photos>();
    }
}
