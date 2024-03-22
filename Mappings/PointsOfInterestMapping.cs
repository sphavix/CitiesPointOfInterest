using AutoMapper;

namespace CityPointOfInterest.Mappings
{
    public class PointsOfInterestMapping : Profile
    {
        public PointsOfInterestMapping()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.CreatePointOfInterestDto, Entities.PointOfInterest>();
            CreateMap<Models.UpdatePointOfInterestDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.UpdatePointOfInterestDto>();
        }
    }
}