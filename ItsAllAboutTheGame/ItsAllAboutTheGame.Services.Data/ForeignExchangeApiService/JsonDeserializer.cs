using System;
using ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService;
using Newtonsoft.Json;

namespace ItsAllAboutTheGame.Services.Data.ForeignExchangeApiService
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public T Deserialize<T>(string jsonString)
        {
            if (string.IsNullOrWhiteSpace(jsonString))
            {
                throw new ArgumentNullException("Deserialization Object cannot be null");
            }

            T objects = JsonConvert.DeserializeObject<T>(jsonString);

            return objects;
        }
    }
}
