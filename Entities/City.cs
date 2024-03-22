using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityPointOfInterest.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}

        [Required]
        [MaxLength(50)]
        public string CityName {get; set;} = string.Empty;

        [MaxLength(200)]  // optional attribute, but good practice to use it.
        public string? Description {get; set;}

        public ICollection<PointOfInterest> PointsOfInterest {get; set;} = new List<PointOfInterest>();

        public City(string cityName)
        {
            CityName = cityName;
        }
    }
}