using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository //As using MongoDb we have to implement IItemsRepository interface like in InMemItemsRepository. And add mongodb dependency with terminal =>"dotnet add package MongoDB.driver".
    {
        //To run MongoDb Container in Docker=> "docker run -d --rm -p 27017:27017 -v mongodbdata:/data/db mongo" -d means we dont want attach to the process. --rm means after process close destroy the container. --name for recognize image easily. -p means opening port for MongoDB can listen it in docker and usually that port is 27017:27017 but right side of port must be 27017. -v is for when docker container stop prevent for losing all data that we stored in MongoDb. Lastly name of the image "mongo".
        //To run MongoDB Container in Docker with authentication=> "docker run -d --rm -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=erayadmin -e MONGO_INITDB_ROOT_PASSWORD=eray#Admin1 mongo".
        //To run MongoDB Container in Docker with authentication and network to connect=> "docker run -d --rm --name mongoNet5 -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=erayadmin -e MONGO_INITDB_ROOT_PASSWORD=eray#Admin1 --network=erayNet5 mongo"
        //To run REST api Container with authentication and same network that belongs MongoDb container=> "docker run -it --rm --name rest_net5 -p 8080:80 -e MongoDbSettings:Host=mongo_net5 -e MongoDbSettings:Password=eray#Admin1 --network=erayNet5 catalog:v1" previously in development our "Host" was "localhost" but now it is MongoDb container's name.
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter; //This will use for filter "Items" in the collection that we want to return.
        private readonly IMongoCollection<Item> itemsCollection; //We want to store here is not client but collection. Collection is the way MongoDB associates all entities together.
        public MongoDbItemsRepository(IMongoClient mongoClient) //To interact with MongoDB, we need MongoDB Client."MongoFb Client" is a component provided by creators of the owners of MongoDB that you can use to interface will be kind of adapter used to interact with MongoDB.
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName); //Create instance, reference to database.
            itemsCollection = database.GetCollection<Item>(collectionName); //Create instance, reference to collection.
        }
        public async Task CreateItemAsync(Item item)//There is a several way to get MongoDB database in out box. First we can install database via MongoDB installer or run database as a Docker container.
        {
           await itemsCollection.InsertOneAsync(item); //Now InsertOne method turned to async type so it's way more efficent and application won't wait this method for finishing its job to continue. Then make all methods "Task" with async and await.
        }
        //Add extension "MongoDb for vsCode". Then before setting our routes we checked MongoDb for if anythings wrong with Postman POST route.
        public async Task DeleteItemAsync(Guid id) //Similar to GetItem(id) but now instead Delete item.
        {
           var filter=filterBuilder.Eq(item=>item.Id, id);
           await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter=filterBuilder.Eq(item=>item.Id, id); //This uses filterBuilder that we declared to filter id from items list.
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await itemsCollection.Find(new BsonDocument()).ToListAsync(); //This will give us list of items in the collection.
        }
        //After changed all methods to async Task model then move to IItemsController.cs
        public async Task UpdateItemAsync(Item item) //Similar to GetItem(id)
        {
            var filter=filterBuilder.Eq(existingItem=>existingItem.Id, item.Id); 
            await itemsCollection.ReplaceOneAsync(filter,item);
        }
    }
}