using AutoMapper;
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
        private readonly IMapper _mapper;

        public CitiesController(ICityRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));  
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public async Task<ActionResult<List<CityWithNoPointsOfInterestDto>>> GetCities()
        {
           var cities = await _repository.GetCitiesAsync();

           return Ok(_mapper.Map<List<CityWithNoPointsOfInterestDto>>(cities));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = await _repository.GetCityAsync(id, includePointsOfInterest);

            if(city is null) return NotFound();

            if(includePointsOfInterest)
            {
                return Ok(_mapper.Map<CityDto>(city));
            }
            return Ok(_mapper.Map<CityWithNoPointsOfInterestDto>(city));
        }
    }
}