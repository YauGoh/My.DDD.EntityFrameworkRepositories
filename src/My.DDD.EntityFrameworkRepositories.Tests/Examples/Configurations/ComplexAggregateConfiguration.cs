using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace My.DDD.EntityFrameworkRepositories.Tests.Examples.Configurations
{
    public class ComplexAggregateConfiguration : IEntityTypeConfiguration<ComplexAggregate>
    {
        public void Configure(EntityTypeBuilder<ComplexAggregate> builder)
        {
            builder.Property<Guid>("FromId");

            builder.HasOne(e => e.From).WithMany().HasForeignKey("FromId").OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.To).WithOne().HasForeignKey("ComplexAggregateId").OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Attachements).WithOne().HasForeignKey("ComplexAggregateId").OnDelete(DeleteBehavior.Cascade);
        }
    }
}
