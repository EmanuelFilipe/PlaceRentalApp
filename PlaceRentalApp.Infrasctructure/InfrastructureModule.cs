using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlaceRentalApp.Core.Repositories;
using PlaceRentalApp.Infrasctructure.Persistence;
using PlaceRentalApp.Infrasctructure.Repositories;

namespace PlaceRentalApp.Infrasctructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddData(configuration)
                    .AddRepositories();

            return services;
        }

        public static IServiceCollection AddData(this IServiceCollection services,
                                                 IConfiguration configuration)
        {
            //builder.Services.AddDbContext<PlaceRentalDbContext>(opt =>
            //    opt.UseInMemoryDatabase("PlaceRentalDbInMemory"));

            var connectionString = configuration.GetConnectionString("PlaceRentalCs");
            services.AddDbContext<PlaceRentalDbContext>(options => 
                options.UseSqlServer(connectionString));

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}
