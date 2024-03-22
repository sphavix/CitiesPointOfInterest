using CityPointOfInterest.Entities;

namespace CityPointOfInterest.Services
{
    public interface ICityRepository
    {
        Task<List<City>> GetCitiesAsync();
        Task<(List<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);
        Task<City?> GetCityAsync(int CityId, bool PointOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int CityId, int PointOfInterestId);

        Task<bool> CityExistsAsync(int CityId);

        Task AddPointOfInterestForCityAsync(int CityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterest(PointOfInterest pointOfInterest);

        Task<bool> SaveChangesAsync();
    }
}