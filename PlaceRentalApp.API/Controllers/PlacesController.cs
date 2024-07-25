using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.API.Entities;
using PlaceRentalApp.API.Models;
using PlaceRentalApp.API.Persistence;
using PlaceRentalApp.API.ValueObjects;

namespace PlaceRentalApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly PlaceRentalDbContext _dbContext;
        public PlacesController(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        [HttpGet("{search}")]
        public IActionResult Get(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = _dbContext
                .Places
                .AsNoTracking()
                .Where(p => p.Title.Contains(search) &&
                !p.IsDeleted &&
                !p.Books.Any(b => (startDate >= b.StartDate && startDate <= b.EndDate) ||
                                  (endDate >= b.StartDate && endDate <= b.EndDate) ||
                                  (startDate <= b.StartDate && endDate >= b.EndDate)));

            return Ok(availablePlaces);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var place = _dbContext.Places.AsNoTracking().SingleOrDefault(p => p.Id == id);

            if (place is null) return NotFound();

            return Ok(place);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreatePlaceInputModel inputModel)
        {
            var address = new Address(
                inputModel.Address.Street,
                inputModel.Address.Number,
                inputModel.Address.ZipCode,
                inputModel.Address.District,
                inputModel.Address.City,
                inputModel.Address.State,
                inputModel.Address.Country);

            var place = new Place(
                inputModel.Title,
                inputModel.Description,
                inputModel.DailyPrice,
                address,
                inputModel.AllowedNumberPerson,
                inputModel.AllowPets,
                inputModel.CreatedBy);

            _dbContext.Places.Add(place);
            _dbContext.SaveChanges();
            
            return CreatedAtAction(nameof(GetById), new { id = place.Id }, inputModel);
        }

        [HttpPut]
        public IActionResult Put(int id, UpdatePlaceInputModel inputModel)
        {
            var place = _dbContext.Places.Find(id);

            if (place is null) return NotFound();

            place.Update(inputModel.Title, inputModel.Description, inputModel.DailyPrice);

            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id:int}/amenities")]
        public IActionResult PostAmenity(int id, CreatePlaceAmenityInputModel inputModel)
        {
            var exists = _dbContext.PlaceAmenities.Any(a => a.Id == id);

            if (!exists) return NotFound();

            var amenity = new PlaceAmenity(inputModel.Description, id);
            _dbContext.PlaceAmenities.Add(amenity);
            _dbContext.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id) 
        {
            var place = _dbContext.Places.Find(id);

            if (place is null) return NotFound();

            place.SetAsDeleted();

            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost("{id:int}/books")]
        public IActionResult PostBooks(int id, CreateBookInputModel inputModel)
        {
            var exists = _dbContext.Places.Any(p => p.Id == id);

            if (!exists) return NotFound();

            var book = new PlaceBook(inputModel.IdUser, inputModel.IdPlace, inputModel.StartDate, 
                                     inputModel.EndDate, inputModel.Comments);

            _dbContext.PlaceBooks.Add(book);
            _dbContext.SaveChanges();
  
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
