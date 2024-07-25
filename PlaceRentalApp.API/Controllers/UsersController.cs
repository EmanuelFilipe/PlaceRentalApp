using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.API.Entities;
using PlaceRentalApp.API.Models;
using PlaceRentalApp.API.Persistence;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PlaceRentalDbContext _dbContext;
        public UsersController(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _dbContext.Users.AsNoTracking().SingleOrDefault(p => p.Id == id);

            if (user is null) return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post(int id, [FromBody] CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate);
            
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, inputModel);
        }
    }
}
