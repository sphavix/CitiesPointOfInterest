using CityPointOfInterest.Models;
using CityPointOfInterest.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityPointOfInterest.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _repository;

        public CitiesController(ICityRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));  
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityWithNoPointsOfInterestDto>>> GetCities()
        {
           var cities = await _repository.GetCitiesAsync();

           var results = new List<CityWithNoPointsOfInterestDto>();
           foreach(var city in cities)
           {
                results.Add(new CityWithNoPointsOfInterestDto
                {
                    Id =city.Id, 
                    Name = city.Name, 
                    Description = city.Description
                });
           }

           return Ok(results);

        }

        // [HttpGet("{id}")]
        // public ActionResult<CityDto> GetCity(int id)
        // {
        //     var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

        //     if(city is null) return NotFound();
        //     return Ok(city);
        // }
    }
}