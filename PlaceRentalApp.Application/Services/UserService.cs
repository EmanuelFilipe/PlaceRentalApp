using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.Repositories;

namespace PlaceRentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        const string NOT_FOUND = "Not found";

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ResultViewModel<UserDetailViewModel?> GetById(int id)
        {
            var user = _userRepository.GetById(id);

            return user is null ? 
                ResultViewModel<UserDetailViewModel>.Error(NOT_FOUND) :
                ResultViewModel<UserDetailViewModel?>.Success(UserDetailViewModel.FromEntity(user));
        }

        public ResultViewModel<int> Insert(CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate);

            _userRepository.Add(user);

            return ResultViewModel<int>.Success(user.Id);
        }
    }
}
