using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace My.DDD.EntityFrameworkRepositories.Tests.Examples.Configurations
{
    public class SimpleAggregateConfiguration : IEntityTypeConfiguration<SimpleAggregate>
    {
        public void Configure(EntityTypeBuilder<SimpleAggregate> builder)
        {
            
        }
    }
}
