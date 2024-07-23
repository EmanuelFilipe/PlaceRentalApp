using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.API.Models;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] CreateUserInputModel inputModel)
        {
            return CreatedAtAction(nameof(GetById), new { id = 1}, inputModel);
        }
    }
}
