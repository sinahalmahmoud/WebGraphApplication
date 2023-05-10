using Google.Apis.Compute.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;

using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;
using WebGraphApplication.Helpers;
using WebGraphApplication.Models;
using WebGraphApplication.ViewModel;



namespace WebGraphApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static GraphServiceClient _graphClient;
 
        public HomeController(ILogger<HomeController> logger, GraphServiceClient graphClient)
        {
            _logger = logger;
            _graphClient = graphClient;
        }
        private static GraphServiceClient GetAuthenticatedGraphClient(IConfigurationRoot config)
        {
            var authenticationProvider = CreateAuthorizationProvider(config);

            _graphClient = new GraphServiceClient(authenticationProvider);
            return _graphClient;
        }
        public IActionResult Index()
        {

            var config = LoadAppSettings();
            if (config == null)
            {
                Console.WriteLine("Invalid appsettings.json file.");
                return View();
            }
            var model = new HomePageViewModel();
            var client = GetAuthenticatedGraphClient(config);


            //sinahs konto
            var Messagesgraphrequest = client.Teams["47294b18-6ee6-4f54-a61f-caef298fe9e9"].Channels["19:9797320d14c845959f39654d1b9cc6cc@thread.tacv2"].Messages.Request().Top(5);
           // prrecio fishbone konto
           //var Messagesgraphrequest = client.Teams["bac562e9-d3d0-4900-a383-e4740c5be1a8"].Channels["19:1e6223cb8c6841e086a36ad6d1c65ea9@thread.skype"].Messages.Request().Top(3);
           // //var Messagesgraphrequest = client.Teams["bac562e9-d3d0-4900-a383-e4740c5be1a8"].AllChannels.Request();

            var MessageResult = Messagesgraphrequest.GetAsync().Result;
            var messages = new List<ChatMessage>();
            var userphotolist = new List<Photos>();
         
       

            foreach (var result in MessageResult)
            {
                Photos userphoto = new Photos();
                if (!string.IsNullOrEmpty(result.Body.Content))
                {
                    messages.Add(result);
                    var userid = result.From.User.Id;
                    // sinahs konto
                    var Replygraphrequest = client.Teams["47294b18-6ee6-4f54-a61f-caef298fe9e9"].Channels["19:9797320d14c845959f39654d1b9cc6cc@thread.tacv2"].Messages[$"{result.Id}"].Replies.Request();
                    //precio fishbone konto
                    //var Replygraphrequest = client.Teams["bac562e9-d3d0-4900-a383-e4740c5be1a8"].Channels["19:1e6223cb8c6841e086a36ad6d1c65ea9@thread.skype"].Messages[$"{result.Id}"].Replies.Request();
                    var Replyresult = Replygraphrequest.GetAsync().Result;
                    model.counts.Add(Replyresult.Count.ToString());

                    ////get photoinformation photo id
                    //var requestUserPhoto = client.Users[$"{userid}"].Photo.Request();
                    //var resultUserPhoto = requestUserPhoto.GetAsync().Result;
                    // get the realphoto
                    var PhotoRequest = client.Users[$"{userid}"].Photo.Content.Request();
                    var PhotoResult = PhotoRequest.GetAsync().Result;
                    //if (PhotoResult != null)
                    //{
                    byte[]  bytes = new byte[PhotoResult.Length];
                        PhotoResult.Read(bytes, 0, (int)PhotoResult.Length);
                        var pic = "data:image/jpeg;charset=utf-8;base64, " + Convert.ToBase64String(bytes);
                        userphoto.userId = userid;
                        userphoto.PhotoURL = pic;
                        userphotolist.Add(userphoto);
                    //}
                    //else
                    //    userphoto.userId = userid;
                    //userphoto.PhotoURL = "./profilePhoto_default";
                    //userphotolist.Add(userphoto);

                }
              
            }
            model.Messages = messages;
            model.photos = userphotolist;
            return View(model);
        }
            
            
          
        
        private static IConfigurationRoot? LoadAppSettings()
        {
            try
            {
                var config = new ConfigurationBuilder()
                                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", false, true)
                                 .Build();

                if (string.IsNullOrEmpty(config["applicationId"]) ||
                    string.IsNullOrEmpty(config["applicationSecret"]) ||
                    string.IsNullOrEmpty(config["redirectUri"]) ||
                    string.IsNullOrEmpty(config["tenantId"])

                    )

                {
                    return null;
                }
                return config;

            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }

        }
        private static IAuthenticationProvider CreateAuthorizationProvider(IConfigurationRoot config)
        {
            var clientId = config["applicationId"];
            var clientSecret = config["applicationSecret"];
            var redirectUri = config["redirectUri"];

            var authority = $"https://login.microsoftonline.com/{config["tenantId"]}/v2.0";



            List<string> scopes = new List<string>();
            scopes.Add("https://graph.microsoft.com/.default");


            var cca = ConfidentialClientApplicationBuilder.Create(clientId)
                                                           .WithAuthority(authority)
                                                           .WithRedirectUri(redirectUri)
                                                           .WithClientSecret(clientSecret)
                                                           .Build();
            return new MsalAuthenticationProvider(cca, scopes.ToArray());


        }










        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}