using System.Net.Http;
using System.Threading.Tasks;
using ItsAllAboutTheGame.Services.External.Contracts;
using ItsAllAboutTheGame.Services.External.Exceptions;

namespace ItsAllAboutTheGame.Services.External
{
    public class ForeignExchangeApiCaller : IForeignExchangeApiCaller
    {
        private IHttpClientFactory clientFactory;

        public ForeignExchangeApiCaller(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public async Task<string> GetCurrencyData(string resourceUrl)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, resourceUrl);
                //Add headers to request here if needed
                var client = clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response
                    .Content
                    .ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new HttpStatusCodeException(response.StatusCode.ToString());
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new HttpRequestException($"Error getting convertion rates from ForeignExchangeApi: {httpRequestException.Message}");
            }
            catch (HttpStatusCodeException httpStatusCodeException)
            {
                throw new HttpStatusCodeException($"ForeignExchangeApi returned status code: {httpStatusCodeException.Message}");
            }
        }
    }
}
