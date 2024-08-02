using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.Application.Services
{
    public interface IUserService
    {
        UserDetailViewModel? GetById(int id);
        int Insert(CreateUserInputModel inputModel);
    }
}
