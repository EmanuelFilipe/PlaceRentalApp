﻿using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Core.ValueObjects;

namespace PlaceRentalApp.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;
        const string NOT_FOUND = "Not found";

        public PlaceService(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public ResultViewModel Book(int id, CreateBookInputModel inputModel)
        {
            var place = _placeRepository.GetById(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            var book = new PlaceBook(inputModel.IdUser, inputModel.IdPlace, inputModel.StartDate,
                                     inputModel.EndDate, inputModel.Comments);

            _placeRepository.AddBook(book);

            return ResultViewModel.Success();
        }

        public ResultViewModel Delete(int id)
        {
            var place = _placeRepository.GetById(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            place.SetAsDeleted();

            _placeRepository.Delete(place);

            return ResultViewModel.Success();
        }

        public ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate)
        {
            var availablePlaces = _placeRepository.GetAllAvailable(search, startDate, endDate) ?? [];

            var model = availablePlaces.Select(PlaceViewModel.FromEntity).ToList();

            return ResultViewModel<List<PlaceViewModel>>.Success(model);
        }

        public ResultViewModel<PlaceDetailsViewModel?> GetById(int id)
        {
            var place = _placeRepository.GetById(id);

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

            int id = _placeRepository.Add(place);

            return ResultViewModel<int>.Success(id);
        }

        public ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel inputModel)
        {
            var place = _placeRepository.GetById(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            var amenity = new PlaceAmenity(inputModel.Description, id);
            
            _placeRepository.AddAmenity(amenity);

            return ResultViewModel.Success();
        }

        public ResultViewModel Update(int id, UpdatePlaceInputModel inputModel)
        {
            var place = _placeRepository.GetById(id);

            if (place is null) return ResultViewModel.Error(NOT_FOUND);

            place.Update(inputModel.Title, inputModel.Description, inputModel.DailyPrice);

            _placeRepository.Update(place);

            return ResultViewModel.Success();
        }

        //public ResultViewModel Cancel(int id)
        //{
        //    throw new NotImplementedException();
        //}

        public ResultViewModel Cancel(int id)
        {
            var place = _placeRepository.GetById(id);

            if (place is null) return ResultViewModel.Error("Not found");

            place.Cancel();

            _placeRepository.Update(place);

            return ResultViewModel.Success();
        }

    }
}
