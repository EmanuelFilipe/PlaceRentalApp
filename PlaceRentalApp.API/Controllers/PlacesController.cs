using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.Services;
using PlaceRentalApp.Infrasctructure.Persistence;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly PlaceRentalDbContext _dbContext;
        private readonly IPlaceService _placeService;

        public PlacesController(PlaceRentalDbContext dbContext, IPlaceService placeService)
        {
            _dbContext = dbContext;
            _placeService = placeService;
        }

        [AllowAnonymous]
        [HttpGet("{search}")]
        public IActionResult Get(string search, DateTime startDate, DateTime endDate)
        {
            var result = _placeService.GetAllAvailable(search, startDate, endDate);
            if (!result.IsSuccess) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "client, admin")]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _placeService.GetById(id);
            if (!result.IsSuccess) return NotFound();
            return Ok(result);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Post([FromBody] CreatePlaceInputModel inputModel)
        {
            if (string.IsNullOrEmpty(inputModel.Title)) return BadRequest();

            var result = _placeService.Insert(inputModel);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, inputModel);
        }

        [HttpPut]
        public IActionResult Put(int id, UpdatePlaceInputModel inputModel)
        {
            var result = _placeService.Update(id, inputModel);

            if (!result.IsSuccess) return NotFound();

            return NoContent();
        }

        [HttpPost("{id:int}/amenities")]
        public IActionResult PostAmenity(int id, CreatePlaceAmenityInputModel inputModel)
        {
            var result = _placeService.InsertAmenity(id, inputModel);

            if (!result.IsSuccess) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
            var result = _placeService.Delete(id);
            if (!result.IsSuccess) return NotFound();
            return NoContent();
        }

        [HttpPost("{id:int}/books")]
        public IActionResult PostBooks(int id, CreateBookInputModel inputModel)
        {
            var result =  _placeService.Book(id, inputModel);
            if (!result.IsSuccess) return NotFound();
            return NoContent();
        }

        [HttpPost("{id:int}/commets")]
        public IActionResult PostComment(int id, CreateCommentInputModel inputModel)
        {
            var place = _dbContext.Places.Find(id);

            if (place is null) return NotFound();

            //var comment = new PlaceBook()
            return NoContent();
        }
    }
}
