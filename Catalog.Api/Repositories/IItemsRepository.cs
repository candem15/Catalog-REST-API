using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;

namespace Catalog.Api.Repositories//Using namespace from folder's name
{
    public interface IItemsRepository//This Interface extracted from InMemItemsRepository then copy pasted to this file.
    {
        Task<IEnumerable<Item>> GetItemsAsync(); //To introduce asynchronous programming model to our REST api we need few changes in repository and controller. Also using refactoring capabilities of vsCode to make all changes across project(Rename symbol=>GetItemsAsync).
        Task<Item> GetItemAsync(Guid id); //Other think we have to do is make sure all methods return "Task" because that is a way to signal this is not synchronous method anymore, but it's going to be asynchronous method.
        Task CreateItemAsync(Item item); //Void methods now turned into Task methods and this interface ready to operate as an asynchronous interface.
        Task UpdateItemAsync(Item item); //Now make changes at MongoDbItemsRepository for async model.
        Task DeleteItemAsync(Guid id);
    }
}