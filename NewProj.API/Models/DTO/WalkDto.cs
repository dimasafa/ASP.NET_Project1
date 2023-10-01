using NewProj.API.Models.Domain;

namespace NewProj.API.Models.DTO
{
    public class WalkDto
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        // Navigation Property
        public DifficultyDto Difficulty { get; set; }
        public RegionDto Region { get; set; }
    }
}
