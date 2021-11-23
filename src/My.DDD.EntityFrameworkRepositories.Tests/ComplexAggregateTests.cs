using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using My.DDD.EntityFrameworkRepositories.Tests.Examples;
using System;
using System.Threading.Tasks;
using Xunit;

namespace My.DDD.EntityFrameworkRepositories.Tests
{
    public class ComplexAggregateTests
    {
        private DbContextOptions<ExampleDbContext> _options;
        private readonly Mock<IEventBus> _mockBus;
        private readonly ExampleDbContext _exampleDbContext;
        private readonly EntityFrameworkRepository<ComplexAggregate> _repository;

        public ComplexAggregateTests()
        {
            _mockBus = new Mock<IEventBus>();

            _options = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _exampleDbContext = new ExampleDbContext(_options);

            _repository = new EntityFrameworkRepository<ComplexAggregate>(_mockBus.Object, _exampleDbContext);
        }

        [Fact]
        public async Task Can_Create_Complex_Aggregate()
        {
            var testAggregate = new ComplexAggregate
            {
                From = new EmailAddress { DisplayName = "Peter Parker", Address = "peter.parker@email.com" },
                To =
                {
                    new EmailAddress { DisplayName = "Tony Starks", Address = "tony.stark@email.com" }
                },
                Subject = "Sorry about the glasses",
                HtmlBody = "...",
                Attachements =
                {
                    new Attachement { Filename = "chocolate.gif", ContentType = "image/gif" },
                }
            };

            await _repository.CreateAsync(testAggregate);

            using var verifyDbContext = new ExampleDbContext(_options);

            var fromDb = await verifyDbContext.ComplexeAggregates
                .Include(_ => _.From)
                .Include(_ => _.To)
                .Include(_ => _.Attachements)
                .SingleAsync(_ => _.Id == testAggregate.Id);

            fromDb.Should().BeEquivalentTo(testAggregate);
        }

        [Fact]
        public async Task Can_Read_Complex_Aggregates()
        {
            var seededAggregate = await SeedDatabase();

            var readAggregate = await _repository.ReadAsync(seededAggregate.Id);

            readAggregate.Should().BeEquivalentTo(seededAggregate);
        }

        [Fact]
        public async Task Can_Update_Complex_Aggregates()
        {
            var seededId = (await SeedDatabase()).Id;

            var aggregate = await _repository.ReadAsync(seededId);
            aggregate!.Subject = "New Subject";
            aggregate.To.Add(new EmailAddress { DisplayName = "Nick Fury", Address = "nick.fury@email.com" });
            aggregate.Attachements.Add(new Attachement { Filename = "SmileyFace.gif", ContentType = "image/gif" });

            await _repository.UpdateAsync(aggregate);

            using var verifyDbContext = new ExampleDbContext(_options);

            var fromDb = await verifyDbContext.ComplexeAggregates
               .Include(_ => _.From)
               .Include(_ => _.To)
               .Include(_ => _.Attachements)
               .SingleAsync(_ => _.Id == aggregate.Id);

            fromDb.Should().BeEquivalentTo(aggregate);
        }

        private async Task<ComplexAggregate> SeedDatabase()
        {
            var testAggregate = new ComplexAggregate
            {
                From = new EmailAddress { DisplayName = "Peter Parker", Address = "peter.parker@email.com" },
                To =
                {
                    new EmailAddress { DisplayName = "Tony Starks", Address = "tony.stark@email.com" }
                },
                Subject = "Sorry about the glasses",
                HtmlBody = "..."
            };

            using var dbContext = new ExampleDbContext(_options);
            dbContext.Add(testAggregate);
            
            await dbContext.SaveChangesAsync();

            return testAggregate;
        }
    }
}
