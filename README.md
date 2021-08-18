# Catalog-REST-API
A REST API maded by using the .NET 5 framework with Test Driven Development. Deployed alongside MongoDB to Kubernetes(Docker).
## Technologies that used on project
C#, .NET 5 Framework, Test Driven Development(xunit), Git, Dependency Injection, Postman, SwaggerUI, CRUD (GET/POST/PUT/DELETE), MongoDB, REST API, Docker, Kubernetes
## Stages of project based to commits
### I. Getting started
#### This is beginning of project.
This commit create after "dotnet new webapi -n catalog",
"dotnet dev-certs https --trust" apply then add ".gitignore" for git
ignored list.
#### Remove WeatherForecast template from project
Also "group" section added to tasks.json for build tasks easier.
### II. Entity, Repository, Controller(GET)
#### Add Entities/Item properties
#### Add Repositories/InMemItemsRepository.cs 
Some items added to repository manually and "GetItem" methods defined.
#### Add Controllers/ItemsController.cs
Configured GET routes for return all items and return one items based of id
### III. Dependency Injection and Data Tranfer Objects(DTOs)
#### Add IItemsRepository.cs and register service 
IItemsRepository interface extracted from InMemItemsRepository for
dependency injection for ItemsController and registered as
singleton service to Startup. For more info check command lines.
#### Add Dtos/ItemDto and Extensions/AsDto extension
Prevent whenever we modify, remove, add "item"
it can potencially break client's "contract".
For that case we using ItemDto instead of Item for client's "contract".
### IV. POST(CREATE), PUT(UPDATE), DELETE
#### Add Dtos/CreateItemDto and POST(CREATE) route
First we add "CreateItem" method in InMemItemsRepository then also
add to our IItemsRepository dependecy beacause it is an "Item".
Add CreateItemDto for taking informartion about "item" that
client posting to server. Then finally adding POST route for
creating new item from client in ItemsController.
#### Add Dtos/UpdateItemDto and PUT(UPDATE) route
First add UpdateItem method to IItemsRepository then
apply it to InMemItemsRepository.
Our UpdateItemDto pretty much same as CreateItemDto but this case may
### V. Persisting Entities with MongoDB
#### Add CRUD operations in MongoDbItemsRepository 
First we changed our old InMemItemsRepository dependecy to
MongoDbItemsRepository in "Startup.cs". Then declared filterBuilder
n MongoDbItemsRepository.cs for filtering our CRUD operations. Now our
routes works with MongoDB.
can change so we create another file to that.
### VI. Task-based asynchronous model => Tasks, Async and Await
#### Apply "Task-based asynchronous model" to REST api
First we modified IItemsRepository.cs to Task-based type. Then applied
task asynchronous to methods in MongoDbItemsRepository.cs and
InMemItemsRepository for learning exercise purpose but we no longer use
InMemItemsRepository dependency. And last changes in ItemsController
CRUD operations and add one option to ItemsController service
in "Startup.cs".
We define UpdateItem in ItemsController with HttpPut(id) attritibute.
#### Add Repositories/MongoDbItemsRepository 
Now we are starting to use Database for our REST api. Firsly add nuget
package "dotnet add package MongoDb.driver" for MongoDb. Then create
new file in Repositories to use MongoDb dependency. Finally implement
IItemsRepository in MongoDbItemsRepository and declare MongoDb Client.
### VII. Secret Manager and Health Checks
#### Add .NET Secret Manager for authentication 
For preventing security issues while connecting MongoDB. ".Net Secret
Manager" store our passwords and not showing in .json files. First our
"docker run" command now includes user and password option =>
"docker run -d --rm -p 27017:27017 -v mongodbdata:/data/db
-e MONGO_INITDB_ROOT_USERNAME=erayadmin -e
MONGO_INITDB_ROOT_PASSWORD=eray#Admin1 mongo". Then add "User",
"Password" properties in MongoDbSettings and also add syntax to
ConnectionString method that MongoDB requires. Further informations are
in comment lines.
#### Add "Health Check" to REST api 
First thing to do is run this command in terminal specificly for MongoDb
HealthChecks "dotnet add package AspNetCore.HealthChecks.MongoDb". Then
go Startup.cs and add HealthChecks services. Atfer that, add endpoints
for HealthChecks services.
### VIII. Docker-Kubernetes
#### Add kubernetes/catalog.yaml deployment
Create kubernetes folder for deployment files. Create catalog.yaml
and follow comment lines on file that we created.
#### Add kubernetes/mongodb.yaml and finish kubernetes
Create new yaml file in kubernetes folder with name mongodb. Then
create statefulSet and it's service. After that apply changes in
terminal to kubernetes. There is much more information at
comment lines that how to do it as like in previous commits.
Finally our REST api and mongodb runs perfectly but now in kubernetes.
### IX. Test Driven Development with xUnit
#### Move REST API into new folder before TDD
At this stage, before creating new project for Unit Tests. We move our
REST API to new "Catalog.Api" folder because we also want "xunit"
project belongs to same level as "Catalog.Api". Then replace following
names with new ones.
1-(Rename File) catalog.csproj => Catalog.Api.csproj
2- catalog.csproj => Catalog.Api.csproj
3- catalog.dll => Catalog.Api.dll
4- namespace catalog => namespace Catalog.Api
5- using catalog => using Catalog.Api
6- At both .vscode/launch.json and .vscode/tasks.json "${workspaceFolder}/" => "${workspaceFolder}/Catalog.Api/"
After all of that switch to new folder then build
"docker build -t candem16/catalog:v2 ." because this is new version of REST API.
#### Add more UnitTests to ItemsControllerTests 
Added all necessary UnitTests inside ItemsControllerTests and
"Fluent Assertion" package to Catalog.UnitTests project. Read the
comment lines to further informations.
#### Polishing REST API to it's final version 
First we changed "item entity"s type to Class and added Description
property. Then make Dtos much simple than old version of itself.
After that modify ItemsControllerTests based to "Entities/Item.cs".
Add GetItemsAsync_WithMatchingItems_ReturnsMatchingItems to
ItemsControllerTests. Build final version of REST API's image to
 push DockerHub.
