using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //Now in this stage of our project we no longer need this repository and not really important to make this async but as learning exercise we doing it.
        public async Task CreateItemAsync(Item item)  //This will add item to list "items".
        {
            items.Add(item);
            await Task.CompletedTask; //We have anything to return in Task its already completed so we used ".CompletedTask".
        }
        public async Task<IEnumerable<Item>> GetItemsAsync() //This will return all items in list "items".
        {
            return await Task.FromResult(items); //In this case we dont have any synchronous method to call so create a Task and return items in Task's result.
        }
        public async Task<Item> GetItemAsync(Guid id) //This will return one item based of "id".
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return await Task.FromResult(item); //Similar changes like in GetItemsAsync.
        }
        public async Task UpdateItemAsync(Item item) //Updating an item based from data that client send.
        {
            var index=items.FindIndex(existingItem=>existingItem.Id==item.Id); //Find existed item's index and match with both's id.
            items[index]=item; //Match an existing item to that updated by client.
            await Task.CompletedTask; //We have anything to return in Task its already completed so we used ".CompletedTask".

        }
        public async Task DeleteItemAsync(Guid id) //Need only id prop of item to delete because its unique.
        {
           var index=items.FindIndex(existingItem=>existingItem.Id==id); //Find existed item's index and match with both's id.
           items.RemoveAt(index); //Remove item from list of items based of index.
           await Task.CompletedTask; //We have anything to return in Task its already completed so we used ".CompletedTask".
        }
    }
}