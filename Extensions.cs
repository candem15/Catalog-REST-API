using catalog.Dtos;
using catalog.Entities;

namespace catalog
{
    public static class Extensions // "Extensions" class must be static for can be used anywhere.
    {
        public static ItemDto AsDto(this Item item)//This extension takes "Item" for changes to "ItemDto" and reduce code repeat.
        {
            return new ItemDto
            {
                Id=item.Id,
                Name=item.Name,
                Price=item.Price,
                CreatedDate=item.CreatedDate
            };
        }
    }
}