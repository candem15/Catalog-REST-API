using System;

namespace Catalog.Api.Entities
{
// Record type used instead of Class type because of *immutable objects,*With-expressions support,*Value-based equality support
    public class Item //After set up our unittests, we can change our REST API without worry to break anything. Also we use class type here because we want mutable objects now.
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}