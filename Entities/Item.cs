using System;

namespace catalog.Entities
{
// Record type used instead of Class type because of *immutable objects,*With-expressions support,*Value-based equality support
    public record Item
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}