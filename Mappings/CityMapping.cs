using AutoMapper;

namespace CityPointOfInterest.Mappings
{
    public class CityMapping : Profile
    {
        public CityMapping()
        {
            CreateMap<Entities.City, Models.CityWithNoPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}