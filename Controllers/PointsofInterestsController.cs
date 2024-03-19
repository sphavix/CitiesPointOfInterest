
using CityPointOfInterest.Models;
using CityPointOfInterest.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityPointOfInterest.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestsController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestsController> _logger;
        private readonly IEmailService _emailService;
        private readonly CitiesDataStore _citiesDataStore;

        public PointsOfInterestsController(ILogger<PointsOfInterestsController> logger, IEmailService emailService,
                CitiesDataStore citiesDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> Get(int cityId)
        {
            try
            {
                var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
                if(city is null)
                {
                    _logger.LogInformation($"City with id {cityId} was not found when accessing points of interests.");
                    return NotFound();
                }
                 return Ok(city.PointsOfInterest);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception thrown when accessing points of interest with id {cityId}", ex);
                return StatusCode(500, "Internal server error, a problem occured while handling your request.");
            }
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city is null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if(pointOfInterest is null) return NotFound();

            return Ok(pointOfInterest);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, CreatePointOfInterestDto pointOfInterest)
        {

            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city is null) return NotFound();

            var maxPointOfInterestId = _citiesDataStore.Cities.SelectMany(x => x.PointsOfInterest).Max(c => c.Id);

            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(finalPointOfInterest);
            return CreatedAtRoute(nameof(GetPointOfInterest), new { cityId = cityId, 
                    pointOfInterestId = finalPointOfInterest.Id }, finalPointOfInterest);
        }

        [HttpPut("{pointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, UpdatePointOfInterestDto pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city is null) return NotFound();

            //find point of interest

            var pointOfInterestToUpdate = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if(pointOfInterestToUpdate is null) return NotFound();

            //perform full update of the record
            pointOfInterestToUpdate.Name = pointOfInterest.Name;
            pointOfInterestToUpdate.Description = pointOfInterest.Description;

            return NoContent(); //204 - no content, 201 - created, 200 - ok, 304 - not modified, 400 - bad request, 401 - unauthorized, 403 - forbidden, 404 - not found, 500 - internal server error, 503 - service unavailable
        }

        [HttpPatch("{pointOfInterestId}")]
        public ActionResult PartialUpdatePointOfInterest(int cityId, int pointOfInterestId, 
            JsonPatchDocument<UpdatePointOfInterestDto> pointOfInterest)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city is null) return NotFound();

            var pointOfInterestToUpdate = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if(pointOfInterestToUpdate is null) return NotFound();

            var pointOfInterestToPatch = new UpdatePointOfInterestDto()
            {
                Name = pointOfInterestToUpdate.Name,
                Description = pointOfInterestToUpdate.Description
            };

            pointOfInterest.ApplyTo(pointOfInterestToPatch, ModelState);

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if(!TryValidateModel(pointOfInterestToPatch)) return BadRequest(ModelState);

            pointOfInterestToUpdate.Name = pointOfInterestToPatch.Name;
            pointOfInterestToUpdate.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public ActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = _citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if(city is null) return NotFound();

            var pointOfInterest = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if(pointOfInterest is null) return NotFound();

            city.PointsOfInterest.Remove(pointOfInterest);

            _emailService.Send("Point of interest has been deleted.", 
                        $"Point of interest {pointOfInterest.Name} with id {pointOfInterest.Id} has been deleted.");

            return NoContent();
        }
    }
}