using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ItsAllAboutTheGame.Services.External
{
    public class ForeignExchangeApiCaller
    {
        public async Task GetRates()
        {
            //HttpClient lets us make web requests from our .NET (C#) Code. To do so it needs to know which url to make the request to.
            using (var client = new HttpClient())
            {
                try
                {
                    var currrencies = new Dictionary<string, int>();
                    client.BaseAddress = new Uri("https://api.exchangeratesapi.io");
                    var response = await client.GetAsync($"/latest?base=USD");
                    //By calling response.EnsureSuccessStatusCode() we can be sure that the request has come back OK.
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawCurrencies = JsonConvert.DeserializeObject<Dictionary<string, int>>(stringResult);
                    Console.WriteLine();
                }
                catch (HttpRequestException httpRequestException)
                {
                    throw new Exception($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }
    }
}
