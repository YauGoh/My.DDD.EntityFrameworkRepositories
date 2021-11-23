using Microsoft.Extensions.DependencyInjection;

namespace My.DDD.EntityFrameworkRepositories
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseEntityFrameworkRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepository<>), typeof(EntityFrameworkRepository<>));

            return services;
        }
    }
}
