using System.ComponentModel.DataAnnotations;

namespace NewProj.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name konnte aus maximal 100 Symbol bestehen")]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000, ErrorMessage = "Description konnte aus maximal 1000 Symbol bestehen")]
        public string Description { get; set; }

        [Required]
        [Range(0, 50)]
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }
    }
}
