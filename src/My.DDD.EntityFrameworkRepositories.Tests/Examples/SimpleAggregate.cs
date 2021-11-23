namespace My.DDD.EntityFrameworkRepositories.Tests.Examples
{
    public class SimpleAggregate : Aggregate
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsDone { get; set; }

    }
}
