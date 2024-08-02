using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Infrasctructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceRentalApp.Infrasctructure.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly PlaceRentalDbContext _dbContext;
        public PlaceRepository(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        public int Add(Place place)
        {
            _dbContext.Places.Add(place);
            _dbContext.SaveChanges();

            return place.Id;
        }

        public void AddAmenity(PlaceAmenity placeAmenity)
        {
            _dbContext.PlaceAmenities.Add(placeAmenity);
            _dbContext.SaveChanges();
        }

        public void AddBook(PlaceBook book)
        {
            _dbContext.PlaceBooks.Add(book);
            _dbContext.SaveChanges();
        }

        public void Delete(Place place)
        {
            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();
        }

        public List<Place>? GetAllAvailable(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = _dbContext.Places
                        .Include(p => p.User)
                        .AsNoTracking()
                        .Where(p => p.Title.Contains(search) &&
                        !p.IsDeleted &&
                        !p.Books.Any(b => (startDate >= b.StartDate && startDate <= b.EndDate) ||
                                          (endDate >= b.StartDate && endDate <= b.EndDate) ||
                                          (startDate <= b.StartDate && endDate >= b.EndDate))).ToList();

            return availablePlaces;
        }

        public Place? GetById(int id)
        {
            var place = _dbContext.Places
                                  .Include(p => p.User)
                                  .Include(p => p.Amenities)
                                  .AsNoTracking().SingleOrDefault(p => p.Id == id);

            return place;
        }

        public void Update(Place place)
        {
            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();
        }
    }
}
