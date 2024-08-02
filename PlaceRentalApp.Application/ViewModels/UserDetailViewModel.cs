using PlaceRentalApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceRentalApp.Application.ViewModels
{
    public class UserDetailViewModel
    {
        public UserDetailViewModel(int id, string fullName, string email, DateTime birthDate)
        {
            Id = id;
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
        }

        public int Id { get; set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }

        public static UserDetailViewModel? FromEntity(User? entity) =>
            entity is null ? null :
            new(
                entity.Id,
                entity.FullName,
                entity.Email,
                entity.BirthDate
            );
    }
}
