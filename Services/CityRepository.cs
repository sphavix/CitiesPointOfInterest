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

        public async Task<List<City>> GetCitiesAsync()
        {
            try
            {
                using(_context)
                {
                    return await _context.Cities.OrderBy(x => x.CityName).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CityExistsAsync(int CityId)
        {
            return await _context.Cities.AnyAsync(x => x.Id == CityId);
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

        public async Task AddPointOfInterestForCityAsync(int CityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(CityId, false);

            if(city is not null)
            {
                city.PointsOfInterest.Add(pointOfInterest);
            }
            await SaveChangesAsync();
        }

        public async void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
            await SaveChangesAsync(); 
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0; // returns true if at least one row is affected.
        }
    }
}