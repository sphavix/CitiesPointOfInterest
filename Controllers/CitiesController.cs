using System.Text.Json;
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

        const int maxPageSize = 20;

        public CitiesController(ICityRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));  
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpGet]
        public async Task<ActionResult<List<CityWithNoPointsOfInterestDto>>> GetCities([FromQuery]string? name, string? searchQuery,
            int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }
           var (cities, paginationMetadata) = await _repository.GetCitiesAsync(name, searchQuery, pageNumber, pageSize);

           //Add the metadat to the response headers
           Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

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