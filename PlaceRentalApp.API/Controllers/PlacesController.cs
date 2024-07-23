using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.API.Models;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string search)
        {
            return Ok();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Post(CreatePlaceInputModel inputModel)
        {
            throw new InvalidDataException();
            return CreatedAtAction(nameof(GetById), new { id = 1 }, inputModel);
        }

        [HttpPut]
        public IActionResult Put(int id, UpdatePlaceInputModel inputModel)
        {
            return NoContent();
        }

        [HttpPost("{id:int}/amenities")]
        public IActionResult PostAmenity(int id, CreatePlaceAmenityInputModel inputModel)
        {
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
            return NoContent();
        }

        [HttpPost("{id:int}/books")]
        public IActionResult PostBooks(int id, CreateBookInputModel inputModel)
        {
            return NoContent();
        }

        [HttpPost("{id:int}/commets")]
        public IActionResult PostComment(int id, CreateCommentInputModel inputModel)
        {
            return NoContent();
        }
    }
}
