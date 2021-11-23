using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace My.DDD.EntityFrameworkRepositories
{
    internal class AggregateIncludeBuilder<TAggregate> where TAggregate : Aggregate
    {
        private readonly List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
        private readonly DbSet<TAggregate> _dbSet;

        private IQueryable<TAggregate> _query;

        public AggregateIncludeBuilder(DbSet<TAggregate> dbSet)
        {
            _query = _dbSet = dbSet;
        }

        public IQueryable<TAggregate> Build()
        {
            _query = _dbSet;

            var traverser = new ComplexEntityTraverser<TAggregate>();
            traverser.OnNavigationPropertyFound += FoundProperty;
            traverser.OnTypeTraversed += TraversedType;

            traverser.Start();

            return _query;
        }

        private void TraversedType(object? sender, Type e)
        {
            if (!propertyInfos.Any()) return;

            propertyInfos.Remove(propertyInfos.Last());
        }

        private void FoundProperty(object? sender, PropertyInfo propertyInfo)
        {
            propertyInfos.Add(propertyInfo);

            _query = _query.Include(GetCurrentPropertyPath());
        }

        private string GetCurrentPropertyPath() => string.Join(".", propertyInfos.Select(pi => pi.Name));
    }
}
