using System.ComponentModel.DataAnnotations;

namespace catalog.Dtos
{
    public record CreateItemDto //While client creating an item, we dont need Id and CreatedDate from client. It will setting up by server.
    {
        [Required] //This is for preventing to create item without a name.
        public string Name { get; init; }
        [Required] //This is for preventing to create item without a price.
        [Range(1,1000)] //This attritibute prevent price's value setting below 0 and max 1000.
        public decimal Price { get; init; }
    }
}