using System.Collections.Generic;

namespace ItsAllAboutTheGame.Services.Data.DTO
{
    public class ForeignExchangeDTO
    {
        public ForeignExchangeDTO()
        {

        }

        public string Base { get; set; }

        public Dictionary<string,decimal> Rates { get; set; }
    }
}
