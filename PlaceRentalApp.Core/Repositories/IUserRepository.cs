using PlaceRentalApp.Core.Entities;

namespace PlaceRentalApp.Core.Repositories
{
    public interface IUserRepository
    {
        User? GetById(int id);
        int Add(User user);
    }
}
