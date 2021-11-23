using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace My.DDD.EntityFrameworkRepositories
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyBaseEntityConfigurations(this ModelBuilder modelBuilder, Assembly domainAssembly)
        {
            var entities = domainAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && typeof(Entity).IsAssignableFrom(t));

            foreach (var entity in entities)
            {
                modelBuilder.Entity(entity).Property(nameof(Entity.Id)).ValueGeneratedNever();
            }

            return modelBuilder;
        }
    }
}
