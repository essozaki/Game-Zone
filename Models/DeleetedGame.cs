namespace GameZone.Models
{
    public class DeleetedGame
    {
        public int Id { get; set; }
        public DateTime DeletedDate { get; set; }
        public int GameId { get; set; }
        public Game DeletedGame { get; set; }
    }
}
