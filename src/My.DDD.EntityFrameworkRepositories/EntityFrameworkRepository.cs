using Microsoft.EntityFrameworkCore;

namespace My.DDD.EntityFrameworkRepositories;
public class EntityFrameworkRepository<TAggregate> : BaseRepository<TAggregate> where TAggregate : Aggregate
{
    private readonly DbContext _dbContext;

    public EntityFrameworkRepository(IEventBus eventBus, DbContext dbContext) : base(eventBus)
    {
        _dbContext = dbContext;
    }

    public override Task<TAggregate?> ReadAsync(Guid id) =>
        IncludeEntireAggregate(_dbContext.Set<TAggregate>())
            .FirstOrDefaultAsync(_ => _.Id == id);
    
    protected override async Task PerformCreateAsync(TAggregate aggregate)
    {
        _dbContext.Set<TAggregate>().Add(aggregate);

        await _dbContext.SaveChangesAsync();
    }

    protected override async Task PerformDeleteAsync(TAggregate aggregate)
    {
        _dbContext.Set<TAggregate>().Remove(aggregate);

        await _dbContext.SaveChangesAsync();
    }

    protected override async Task PerformUpdateAsync(TAggregate aggregate)
    {
        _dbContext.Set<TAggregate>().Update(aggregate);

        await _dbContext.SaveChangesAsync();
    }

    protected virtual IQueryable<TAggregate> IncludeEntireAggregate(DbSet<TAggregate> dbSet) => new AggregateIncludeBuilder<TAggregate>(dbSet).Build();
}
