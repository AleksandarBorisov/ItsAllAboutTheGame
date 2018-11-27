using ItsAllAboutTheGame.Data.Models;
using System.Net.Http;

namespace ItsAllAboutTheGame.Services.External
{
    public class PaySaveApiCaller
    {
        private IHttpClientFactory clientFactory;

        public PaySaveApiCaller(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public void CreateProfile(User user)
        {
            //In the test environment, only the card numbers provided by PaySave will work properly. 

        }
    }
}
