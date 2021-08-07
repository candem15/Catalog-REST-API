using System;
using System.Collections.Generic;
using System.Linq;
using catalog.Entities;

namespace catalog.Repositories
{
    public class InMemItemsRepository : IItemsRepository//Repository that implements interface from IItemsRepository file.
    {
        private readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 25, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Wooden Shield", Price = 16, CreatedDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Twin Dagger", Price = 40, CreatedDate = DateTimeOffset.UtcNow },
        };

        public void CreateItem(Item item)  //This will add item to list "items".
        {
            items.Add(item);
        }

        public IEnumerable<Item> GetItems() //This will return all items in list "items".
        {
            return items;
        }
        public Item GetItem(Guid id) //This will return one item based of "id".
        {
            return items.Where(item => item.Id == id).SingleOrDefault();
        }
        public void UpdateItem(Item item) //Updating an item based from data that client send.
        {
            var index=items.FindIndex(existingItem=>existingItem.Id==item.Id); //Find existed item's index and match with both's id.
            items[index]=item; //Match an existing item to that updated by client.
        }

        public void DeleteItem(Guid id) //Need only id prop of item to delete because its unique.
        {
           var index=items.FindIndex(existingItem=>existingItem.Id==id); //Find existed item's index and match with both's id.
           items.RemoveAt(index); //Remove item from list of items based of index.
        }
    }

}