using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace CityPointOfInterest.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]

        public string PointOfInterestName { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("CityId")]  //foreign key for city table.
        public City? City { get; set; }
        public int CityId { get; set; }

        public PointOfInterest(string pointOfInterestName)
        {
            PointOfInterestName = pointOfInterestName;
        }
    }
}