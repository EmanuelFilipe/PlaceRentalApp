using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Infrasctructure.Auth;

namespace PlaceRentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        const string NOT_FOUND = "Not found";

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
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
            var hash = _authService.ComputeHash(inputModel.Password);

            var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate,
                                hash, inputModel.Role);

            _userRepository.Add(user);

            return ResultViewModel<int>.Success(user.Id);
        }

        public ResultViewModel<LoginViewModel> Login(LoginInputModel inputModel)
        {
            var hash = _authService.ComputeHash(inputModel.Password);

            var user = _userRepository.GetByLoginAndHash(inputModel.Email, hash);

            if (user is null)
                return ResultViewModel<LoginViewModel?>.Error("Error");

            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            return ResultViewModel<LoginViewModel?>.Success(viewModel);
        }
    }
}
