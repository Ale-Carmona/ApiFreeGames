namespace ApiFreeGames.Models
{
    public class GameModel
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? thumbnail { get; set; }
        public string? short_description { get; set; }
        public string? game_url { get; set; }
        public string? genre { get; set; }
        public string? platform { get; set; }
        public string? publisher { get; set; }
        public string? developer { get; set; }
        public string? release_date { get; set; }
        public string? profile_url { get; set; }


        // ----------- CAMPOS EXTRA DEL GET BY ID -----------
        public string? status { get; set; }
        public string? description { get; set; }

    }
}
