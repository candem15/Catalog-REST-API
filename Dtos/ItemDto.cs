using System;

namespace catalog.Dtos //(D)ata (T)ransfer (O)bject using for prevent whenever we modify, remove, add "item" it can potencially break client's "contract(in this case this is item object)". 
{
    public record ItemDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}