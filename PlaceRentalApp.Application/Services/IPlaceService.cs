using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.Application.Services
{
    public interface IPlaceService
    {
        ResultViewModel<List<PlaceViewModel>> GetAllAvailable(string search, DateTime startDate, DateTime endDate);
        ResultViewModel<PlaceDetailsViewModel?> GetById(int id);
        ResultViewModel<int> Insert(CreatePlaceInputModel inputModel);
        ResultViewModel InsertAmenity(int id, CreatePlaceAmenityInputModel inputModel);
        ResultViewModel Book(int id, CreateBookInputModel inputModel);
        ResultViewModel Update(int id, UpdatePlaceInputModel inputModel);
        ResultViewModel Delete(int id);

    }

}
