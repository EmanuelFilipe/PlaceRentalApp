using Microsoft.AspNetCore.Mvc;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.Services;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = _userService.GetById(id);
            if(!result.IsSuccess) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateUserInputModel inputModel)
        {
            if (string.IsNullOrEmpty(inputModel.Password))
                return BadRequest();

            var result = _userService.Insert(inputModel);
            return CreatedAtAction(nameof(GetById), new { id = result.Data }, inputModel);
        }

        [HttpPut("login")]
        public IActionResult Login(LoginInputModel inputModel)
        {
            var result = _userService.Login(inputModel);

            if (!result.IsSuccess) return BadRequest();

            return Ok(result);
        }
    }
}
