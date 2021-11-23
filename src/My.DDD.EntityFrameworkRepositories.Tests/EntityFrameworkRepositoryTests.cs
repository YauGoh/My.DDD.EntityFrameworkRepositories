using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using My.DDD.EntityFrameworkRepositories.Tests.Examples;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace My.DDD.EntityFrameworkRepositories.Tests
{
    public class EntityFrameworkRepositoryTests
    {
        private DbContextOptions<ExampleDbContext> _options;
        private readonly Mock<IEventBus> _mockBus;
        private readonly ExampleDbContext _exampleDbContext;
        private readonly EntityFrameworkRepository<SimpleAggregate> _repository;

        public EntityFrameworkRepositoryTests()
        {
            _mockBus = new Mock<IEventBus>();

            _options = new DbContextOptionsBuilder<ExampleDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _exampleDbContext = new ExampleDbContext(_options);

            _repository = new EntityFrameworkRepository<SimpleAggregate>(_mockBus.Object, _exampleDbContext);
        }

        [Fact]
        public async Task Creates_an_aggragates()
        {
            await _repository.CreateAsync(new SimpleAggregate { Title = "Clean room" });

            using var verifyDbContext = new ExampleDbContext(_options);

            verifyDbContext.SimpleAggregates.First().Title.Should().Be("Clean room");
        }

        [Fact]
        public async Task Reads_an_aggregate()
        {
            var testAggregate = new SimpleAggregate { Title = "Existing" };

            using var seedDbContext = new ExampleDbContext(_options);
            seedDbContext.Add(testAggregate);
            await seedDbContext.SaveChangesAsync();

            var aggregate = await _repository.ReadAsync(testAggregate.Id);

            aggregate.Should().BeEquivalentTo(testAggregate);
        }

        [Fact]
        public async Task Updates_an_aggregate()
        {
            var existingAggregateId = await SeedDataBase();

            var aggregate = await _repository.ReadAsync(existingAggregateId);
            aggregate!.Title = "New Title";
            aggregate!.Description = "New Description";

            await _repository.UpdateAsync(aggregate);

            using var verifyDbContext = new ExampleDbContext(_options);

            var updated = verifyDbContext.SimpleAggregates.Single(_ => _.Id == existingAggregateId);

            updated.Title.Should().Be("New Title");
            updated.Description.Should().Be("New Description");
        }

        [Fact]
        public async Task Deletes_an_aggregate()
        {
            var existingAggregateId = await SeedDataBase();

            var aggregate = await _repository.ReadAsync(existingAggregateId);
           
            await _repository.DeleteAsync(aggregate!);

            using var verifyDbContext = new ExampleDbContext(_options);

            verifyDbContext.SimpleAggregates.Should().NotContain(_ => _.Id == existingAggregateId);
        }

        private async Task<Guid> SeedDataBase()
        {
            var testAggregate = new SimpleAggregate { Title = "Existing" };

            using var seedDbContext = new ExampleDbContext(_options);
            seedDbContext.Add(testAggregate);
            await seedDbContext.SaveChangesAsync();

            return testAggregate.Id;
        }
    }
}
