
using System.Globalization;
using AutoMapper;
using CityPointOfInterest.Models;
using CityPointOfInterest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityPointOfInterest.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    [Authorize]
    public class PointsOfInterestsController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestsController> _logger;
        private readonly IEmailService _emailService;
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IEmailService emailService,
                ICityRepository cityRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation($"City with id {cityId} not found");
                return NotFound();
            }
            
            var pointOfInterestForCity = await _cityRepository.GetPointsOfInterestForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestForCity));
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            if(pointOfInterest is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, CreatePointOfInterestDto pointOfInterest)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            await _cityRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);

            await _cityRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new {cityId, pointOfInterestId = createdPointOfInterestToReturn.Id},
                            createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, UpdatePointOfInterestDto pointOfInterest)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            //find point of interest
            var pointInterestEntity = await _cityRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if(pointInterestEntity is null) return NotFound();

            //perform full update of the record
            _mapper.Map(pointOfInterest, pointInterestEntity);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartialUpdatePointOfInterest(int cityId, int pointOfInterestId, 
            JsonPatchDocument<UpdatePointOfInterestDto> pointOfInterest)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointInterestEntity = await _cityRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if(pointInterestEntity is null) return NotFound();

            var pointOfInterestToPatch = _mapper.Map<UpdatePointOfInterestDto>(pointInterestEntity);

            pointOfInterest.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            _mapper.Map(pointOfInterestToPatch, pointInterestEntity);

            await _cityRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task< ActionResult> DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            if(!await _cityRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointInterestEntity = await _cityRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);
            if(pointInterestEntity is null) return NotFound();

            _cityRepository.DeletePointOfInterest(pointInterestEntity);

            await _cityRepository.SaveChangesAsync();

            _emailService.Send("Point of interest has been deleted.", 
                        $"Point of interest {pointInterestEntity.PointOfInterestName} with id {pointInterestEntity.Id} has been deleted.");

            return NoContent();
        }
    }
}