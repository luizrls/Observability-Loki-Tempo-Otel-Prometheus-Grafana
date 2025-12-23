using APIContagem.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace APIContagem.Api.Configurations.Database
{
    internal static class DataBaseConfig
    {
        internal static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("BaseContagem"));
            });

            return services;
        }
    }
}
