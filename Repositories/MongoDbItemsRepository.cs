using System;
using System.Collections.Generic;
using catalog.Entities;
using MongoDB.Driver;

namespace catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository //As using MongoDb we have to implement IItemsRepository interface like in InMemItemsRepository. And add mongodb dependency with terminal =>"dotnet add package MongoDB.driver".
    {
        //MongoDb Container in Docker=>"docker run -d --rm -p 27017:27017 -v mongodbdata:/data/db mongo" -d means we dont want attach to the process. --rm means after process close destroy the container. --name for recognize image easily. -p means opening port for MongoDB can listen it in docker and usually that port is 27017:27017 but right side of port must be 27017. -v is for when docker container stop prevent for losing all data that we stored in MongoDb. Lastly name of the image "mongo".
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection; //We want to store here is not client but collection. Collection is the way MongoDB associates all entities together.
        public MongoDbItemsRepository(IMongoClient mongoClient) //To interact with MongoDB, we need MongoDB Client."MongoFb Client" is a component provided by creators of the owners of MongoDB that you can use to interface will be kind of adapter used to interact with MongoDB.
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName); //Create instance, reference to database.
            itemsCollection = database.GetCollection<Item>(collectionName); //Create instance, reference to collection.
        }
        public void CreateItem(Item item)//There is a several way to get MongoDB database in out box. First we can install database via MongoDB installer or run database as a Docker container.
        {
            itemsCollection.InsertOne(item);
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