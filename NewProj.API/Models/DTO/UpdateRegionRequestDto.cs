using System.ComponentModel.DataAnnotations;

namespace NewProj.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code muss aus mindestens 3 Symbol bestehen")]
        [MaxLength(3, ErrorMessage = "Code muss aus maximal 3 Symbol bestehen")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name konnte aus maximal 100 Symbol bestehen")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
