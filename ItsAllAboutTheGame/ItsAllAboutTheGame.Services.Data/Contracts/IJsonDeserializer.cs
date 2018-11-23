namespace ItsAllAboutTheGame.Services.Data.Contracts.ForeignExchangeApiService
{
    public interface IJsonDeserializer
    {
        T Deserialize<T>(string jsonString);
    }
}