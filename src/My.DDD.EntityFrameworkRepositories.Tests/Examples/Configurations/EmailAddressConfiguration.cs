using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace My.DDD.EntityFrameworkRepositories.Tests.Examples.Configurations
{
    public class EmailAddressConfiguration : IEntityTypeConfiguration<EmailAddress>
    {
        public void Configure(EntityTypeBuilder<EmailAddress> builder)
        {
            builder.Property<Guid?>("ComplexAggregateId");
        }
    }
}
