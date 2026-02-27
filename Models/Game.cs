
namespace GameZone.Models
{
    public class Game: BaseEntity
    {
        
        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Cover { get; set; } = string.Empty;
        [MaxLength(20)]
        public string price { get; set; } = default;
        public int CategoryId { get; set; }
        public bool isDeleted { get; set; } = false;
        //relations
        public Category category { get; set; } = default;
        public ICollection<GameDevice> Device { get; set; } = new List<GameDevice>();
        //public DeleetedGame DeleetedGame { get; set; } = default;
    }
}
