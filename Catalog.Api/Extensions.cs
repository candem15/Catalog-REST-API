using Catalog.Api.Dtos;
using Catalog.Api.Entities;

namespace Catalog.Api
{
    public static class Extensions // "Extensions" class must be static for can be used anywhere.
    {
        public static ItemDto AsDto(this Item item)//This extension takes "Item" for changes to "ItemDto" and reduce code repeat.
        {
            return new ItemDto(item.Id, item.Name, item.Price, item.Description, item.CreatedDate); //Description added in "Item" entity so needs to add in "ItemDto" also. We changed our Item.cs entity's type to class but we still want our ItemDto immutable. We must create ItemDto via constructor so after construction nobody can change DTO's properties.
        }
    }
}