using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;

namespace PlaceRentalApp.Application.Services
{
    public interface IUserService
    {
        ResultViewModel<UserDetailViewModel?> GetById(int id);
        ResultViewModel<int> Insert(CreateUserInputModel inputModel);
    }
}
