using APIContagem.Api.Data;

namespace APIContagem.Api.Configurations.Database
{
    internal static class RepositoriesConfig
    {
        internal static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CounterRepository>();

            return services;
        }
    }
}
