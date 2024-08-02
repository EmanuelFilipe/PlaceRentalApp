using Microsoft.Extensions.DependencyInjection;
using PlaceRentalApp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaceRentalApp.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
