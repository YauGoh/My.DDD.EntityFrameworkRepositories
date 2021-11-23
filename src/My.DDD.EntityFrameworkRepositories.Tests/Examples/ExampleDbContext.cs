using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace My.DDD.EntityFrameworkRepositories.Tests.Examples
{
    public class ExampleDbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ExampleDbContext(DbContextOptions options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyBaseEntityConfigurations(Assembly.GetExecutingAssembly());
        }

        public DbSet<ComplexAggregate> ComplexeAggregates { get; set; }

        public DbSet<SimpleAggregate> SimpleAggregates { get; set; }
    }
}
