using System.Collections.Generic;

namespace My.DDD.EntityFrameworkRepositories.Tests.Examples
{
    public class ComplexAggregate : Aggregate
    {
        public ComplexAggregate()
        {
            To = new HashSet<EmailAddress>();

            Attachements = new HashSet<Attachement>();
        }

        public EmailAddress? From { get; set; }

        public ICollection<EmailAddress> To { get; set; }

        public string? Subject { get; set; }

        public string? HtmlBody { get; set; }

        public ICollection<Attachement> Attachements { get; set; }
    }

    public class EmailAddress : Entity
    {
        public string? DisplayName { get; set; }

        public string? Address { get; set; }
    }

    public class Attachement : Entity
    {
        public string? Filename { get; set; }

        public string? ContentType { get; set; }

        public byte[]? Content { get; set; }
    }
}
