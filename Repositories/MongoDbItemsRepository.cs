using System;
using System.Collections.Generic;
using catalog.Entities;
using MongoDB.Driver;

namespace catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository //As using MongoDb we have to implement IItemsRepository interface like in InMemItemsRepository. And add mongodb dependency with terminal =>"dotnet add package MongoDB.driver".
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection; //We want to store here is not client but collection. Collection is the way MongoDB associates all entities together.
        public MongoDbItemsRepository(IMongoClient mongoClient) //To interact with MongoDB, we need MongoDB Client."MongoFb Client" is a component provided by creators of the owners of MongoDB that you can use to interface will be kind of adapter used to interact with MongoDB.
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName); //Create instance, reference to database.
            itemsCollection = database.GetCollection<Item>(collectionName); //Create instance, reference to collection.
        }
        public void CreateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}