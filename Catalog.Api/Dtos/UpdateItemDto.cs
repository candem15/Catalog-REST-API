using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public record UpdateItemDto // This is pretty much same as CreateItemDto in this case.
    {
        [Required] //This is for preventing to update item without a name.
        public string Name { get; init; }
        [Required] //This is for preventing to update item without a price.
        [Range(1,1000)] //This attritibute prevent price's value setting below 0 and max 1000.
        public decimal Price { get; init; }
    }
}