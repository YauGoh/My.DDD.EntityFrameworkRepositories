using System.Collections;
using System.Reflection;

namespace My.DDD.EntityFrameworkRepositories
{
    internal class ComplexEntityTraverser<TEntity> where TEntity : Entity
    {
        private List<Type> traversedType = new List<Type>();

        public event EventHandler<PropertyInfo>? OnNavigationPropertyFound;

        public event EventHandler<Type>? OnTypeTraversed;

        public void Start()
        {
            traversedType.Clear();

            TraverseType(typeof(TEntity));
        }

        private void TraverseType(Type type)
        {
            // don't traverse a type more than once, to avoid cyclic loops
            if (traversedType.Contains(type)) return;

            traversedType.Add(type);

            var publicProperties = type.GetProperties();

            foreach (var publicProperty in publicProperties)
            {
                if (publicProperty.PropertyType.IsAssignableTo(typeof(Entity)))
                {
                    OnNavigationPropertyFound?.Invoke(this, publicProperty);

                    TraverseType(publicProperty.PropertyType);

                    OnTypeTraversed?.Invoke(this, publicProperty.PropertyType);
                }
                else if (IsGenericCollectionOfEntities(publicProperty, out var entityType))
                {
                    OnNavigationPropertyFound?.Invoke(this, publicProperty);

                    TraverseType(entityType!);

                    OnTypeTraversed?.Invoke(this, entityType);
                }
            }
        }

        private static bool IsGenericCollectionOfEntities(PropertyInfo publicProperty, out Type? entityType)
        {
            entityType = null;

            var isGenericCollectionOfEntities = 
                publicProperty.PropertyType.IsAssignableTo(typeof(IEnumerable)) && 
                publicProperty.PropertyType.IsGenericType && 
                publicProperty.PropertyType.GetGenericArguments().First().IsAssignableTo(typeof(Entity));

            if (isGenericCollectionOfEntities)
            {
                entityType = publicProperty.PropertyType.GetGenericArguments().First();
            }

            return isGenericCollectionOfEntities;
        }
    }
}
