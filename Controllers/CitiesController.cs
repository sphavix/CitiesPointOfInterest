using CityPointOfInterest.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityPointOfInterest.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities); // return Ok(CitiesDataStore.Current.Cities.ToList() as IEnumerable<CityDto> ?? new List<CityDto>(0) as IEnumerable<CityDto> ?? new List<CityDto>(0) as IEnumerable<CityDto> ?? new List<
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if(city is null) return NotFound();
            return Ok(city);
        }
    }
}