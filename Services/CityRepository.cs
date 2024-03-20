using CityPointOfInterest.DataContext;
using CityPointOfInterest.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityPointOfInterest.Services
{
    public class CityRepository : ICityRepository
    {
        private readonly CityInfoDbContext _context;
        public CityRepository(CityInfoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int CityId, bool PointOfInterest)
        {
            if(PointOfInterest)
            {
                return await _context.Cities.Include(x => x.PointsOfInterest)
                        .Where(x => x.Id == CityId).FirstOrDefaultAsync();
            }

            return await _context.Cities.Where(x => x.Id == CityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest> GetPointOfInterestForCityAsync(int CityId, int PointOfInterestId)
        {
            return await _context.PointsOfInterest.Where(x => x.CityId == CityId && x.Id == PointOfInterestId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await _context.PointsOfInterest.Where(x => x.CityId == cityId).ToListAsync();
        }
    }
}