

using ApiFreeGames.Models;
using Newtonsoft.Json;

namespace ApiFreeGames.Services
{
    public class GameService
    {
        private readonly HttpClient _httpClient;

        public GameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //GET ALL
        public async Task<List<GameModel>> GetAllGames()
        {
            try
            {
                string url = "https://api.codetabs.com/v1/proxy?quest=https://www.freetogame.com/api/games";

                string json = await _httpClient.GetStringAsync(url);

                return JsonConvert.DeserializeObject<List<GameModel>>(json)
                       ?? new List<GameModel>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consumir API pública: " + ex.Message);
            }
        }


        // GET BY ID
        public async Task<GameModel?> GetGameById(int id)
        {
            string url = $"https://api.codetabs.com/v1/proxy?quest=https://www.freetogame.com/api/game?id={id}";

            string json = await _httpClient.GetStringAsync(url);

            return JsonConvert.DeserializeObject<GameModel>(json);
        }

    }
}
