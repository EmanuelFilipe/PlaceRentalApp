using Microsoft.EntityFrameworkCore;
using PlaceRentalApp.Application.Exceptions;
using PlaceRentalApp.Application.Models;
using PlaceRentalApp.Application.ViewModels;
using PlaceRentalApp.Core.Entities;
using PlaceRentalApp.Infrasctructure.Persistence;

namespace PlaceRentalApp.Application.Services
{
    public class UserService : IUserService
    {
        private readonly PlaceRentalDbContext _dbContext;
        public UserService(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        public UserDetailViewModel GetById(int id)
        {
            var user = _dbContext.Users.AsNoTracking().SingleOrDefault(p => p.Id == id);

            if (user is null) throw new NotFoundExceptions();

            var model = UserDetailViewModel.FromEntity(user);

            return model;
        }

        public int Insert(CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.BirthDate);

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }
    }
}
