using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Application.Exceptions;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.ValueObjects;
using PlaceRentalApp.Infrasctructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceRentalApp.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly PlaceRentalDbContext _dbContext;
        const string NOT_FOUND = "Not found";
        public PlaceService(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        public ResultViewModel Book(int id, CreateBookInputModel inputModel)
        {
            var exists = _dbContext.Places.Any(p => p.Id == id);

            if (!exists) return ResultViewModel.Error(NOT_FOUND);

            var book = new PlaceBook(inputModel.IdUser, inputModel.IdPlace, inputModel.StartDate,
                                     inputModel.EndDate, inputModel.Comments);

            _dbContext.PlaceBooks.Add(book);
            _dbContext.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel Delete(int id)
        {
            var place = _dbContext.Places.Find(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            place.SetAsDeleted();

            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = 
                _dbContext.Places
                          .Include(p => p.User)
                          .AsNoTracking()
                          .Where(p => p.Title.Contains(search) &&
                          !p.IsDeleted &&
                          !p.Books.Any(b => (startDate >= b.StartDate && startDate <= b.EndDate) ||
                                            (endDate >= b.StartDate && endDate <= b.EndDate) ||
                                            (startDate <= b.StartDate && endDate >= b.EndDate))).ToList();

            var model = availablePlaces.Select(PlaceViewModel.FromEntity).ToList();

            return ResultViewModel<List<PlaceViewModel>>.Success(model);
        }

        public ResultViewModel<PlaceDetailsViewModel?> GetById(int id)
        {
            var place = _dbContext.Places
                                  .Include(p => p.User)  
                                  .Include(p => p.Amenities)
                                  .AsNoTracking().SingleOrDefault(p => p.Id == id);

            return place is null ?
                ResultViewModel<PlaceDetailsViewModel?>.Error(NOT_FOUND) :
                ResultViewModel<PlaceDetailsViewModel?>.Success(PlaceDetailsViewModel.FromEntity(place));
        }

        public ResultViewModel<int> Insert(CreatePlaceInputModel inputModel)
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

            return ResultViewModel<int>.Success(place.Id);
        }

        public ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel inputModel)
        {
            var exists = _dbContext.PlaceAmenities.Any(a => a.Id == id);

            if (!exists) return ResultViewModel.Error(NOT_FOUND);

            var amenity = new PlaceAmenity(inputModel.Description, id);
            _dbContext.PlaceAmenities.Add(amenity);
            _dbContext.SaveChanges();

            return ResultViewModel.Success();
        }

        public ResultViewModel Update(int id, UpdatePlaceInputModel inputModel)
        {
            var place = _dbContext.Places.Find(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            place.Update(inputModel.Title, inputModel.Description, inputModel.DailyPrice);

            _dbContext.Places.Update(place);
            _dbContext.SaveChanges();

            return ResultViewModel.Success();
        }
    }
}
