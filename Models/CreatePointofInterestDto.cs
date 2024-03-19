using System.ComponentModel.DataAnnotations;

namespace CityPointOfInterest.Models
{
    public class CreatePointOfInterestDto
    {
        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Description { get; set; }
    }
}