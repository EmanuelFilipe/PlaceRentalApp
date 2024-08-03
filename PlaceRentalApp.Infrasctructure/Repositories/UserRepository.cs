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
    public class UserRepository : IUserRepository
    {
        private readonly PlaceRentalDbContext _dbContext;
        public UserRepository(PlaceRentalDbContext context)
        {
            _dbContext = context;
        }

        public int Add(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public User? GetById(int id)
        {
            var user = _dbContext.Users.AsNoTracking().SingleOrDefault(p => p.Id == id);
            return user;
        }

        public User? GetByLoginAndHash(string email, string hash)
        {
            var user = _dbContext.Users.AsNoTracking().SingleOrDefault(p => p.Email == email &&
                                                                       p.Password == hash);
            return user;
        }
    }
}
