using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Catalog.Api.Dtos;
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.Generic;
//If you are getting red underlines after rename this file to "ItemsControllerTests" press f1 and restart OmniSharp.
namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {   
        private readonly Mock<IItemsRepository> repositoryStub = new(); //We define theese properties inside class because to preventing code repeat. And turned them to comment line.
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random rand = new ();

        [Fact] //This attritibute used for declare that one of the methods in this class is test method.
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound() //Naming unit tests is important because it helps to identify what is this test supposed to do. In this case we are using "following" method for naming unit tests => "UnitofWork_StateUnderTest_ExpectedBehavior".
        {
            //Arrange =>  In this section we set up everything to be ready to execute the test.
            
            //var repositoryStub = new Mock<IItemsRepository>(); //We are using fake respository(Stub) for unit test because we dont want to test actual repository but only methods inside ItemsController.
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())) //In here doesn't matter what's the item that we are going to be passing because we are testing unexisted item.
                .ReturnsAsync((Item)null); //And we cast this into the item entity because Mock get confused. 
            
            //var loggerStub = new Mock<ILogger<ItemsController>>();

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            
            //Act => In this section we execute the test.

            var result = await controller.GetItemAsync(Guid.NewGuid()); //Like we said it is not important what guid we pass because we are testing "return NotFound()" action.
            
            //Assert => In this section we verify whatever needs to be verified about the execution of the unit test.
        
            //Assert.IsType<NotFoundResult>(result.Result); //"Result" property inside result represents the actual result that actual type of result we got, which in our case, it should be "Not Found".
            result.Result.Should().BeOfType<NotFoundResult>(); //This is fluent version of assert.
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem() 
        {
            //Arrange
            
            Item expectedItem = CreateRandomItem();

            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())) //This line pretty much same as "GetItemAsync_WithUnexistingItem_ReturnsNotFound". But here we return random item.
                .ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            var result = await controller.GetItemAsync(Guid.NewGuid());

            //Assert

            result.Value.Should().BeEquivalentTo(expectedItem); //This methods comes from "Fluent Assetions".

        }

         [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems() //Similar like "GetItemAsync_WithExistingItem_ReturnsExistedItem".
        {
            //Arrange
            
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

            repositoryStub.Setup(repo => repo.GetItemsAsync()) 
                .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            var result = await controller.GetItemsAsync();

            //Assert

            result.Should().BeEquivalentTo(expectedItems); 

        }

        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems() //Getting all items that contains given name.
        {
            //Arrange
            
            var allItems = new[]
            {
            new Item() { Name= "Great Axe"},
            new Item() { Name= "Blade Axe"},
            new Item() { Name= "Two-Handed Sword"}
            };

            var nameToMatch = "Axe";
            repositoryStub.Setup(repo => repo.GetItemsAsync()) 
                .ReturnsAsync(allItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            //Assert

            foundItems.Should().OnlyContain(
                item => item.Name == allItems[0].Name || item.Name == allItems[1].Name
                );

        }

        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            //Arrange
            
            var itemToCreate = new CreateItemDto( //While creating item as you remember we need to give "name", "price" and "description".
                Guid.NewGuid().ToString(),
                rand.Next(1000),
                Guid.NewGuid().ToString()); 

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            var result = await controller.CreateItemAsync(itemToCreate);

            //Assert

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo( 
                createdItem,
                options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers() //"ItemDto" at Catalog.Api has 4 properties but "itemToCreate" got only 2. Because of that added additional filter for comparing without missing members.
                ); 
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow,TimeSpan.FromMilliseconds(1000)); //CreatedDate should be close to current time because it is super fast. In this case we set up requared difference between CreatedDate and current time to maximum 1 second for pass the test. 
        }

        [Fact]
        public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
        {
            //Arrange

            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
            
            var itemId = existingItem.Id; // We took existingItem's id for update.
            var itemToUpdate = new UpdateItemDto( //Here we give new values to properties for simulate updating item.
                Guid.NewGuid().ToString(),
                rand.Next(1000),
                Guid.NewGuid().ToString()); 
           
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);
            
            //Assert

            result.Should().BeOfType<NoContentResult>();

        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            //Arrange

            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())) 
                .ReturnsAsync(existingItem);
            
            var itemId = existingItem.Id; // We took existingItem's id for delete.

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act

            var result = await controller.DeleteItemAsync(itemId);
            
            //Assert

            result.Should().BeOfType<NoContentResult>();

        }

        private Item CreateRandomItem() //For testing an existing item we created handy method that will use over inside of unit tests.
        {
            return new() //We don't care about much what are the values of properties. But we have to give values right type and range like actual "Item".
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rand.Next(1000),
                Description = Guid.NewGuid().ToString(),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }   
    }
}
//There is extension to visualize the overall status of our test suite. Like, which case is passing, which words are failing and so on. To do that we added ".NET Core Test Explorer" extension. Then click gear icon and go extension settings, switch to workspace section from user and find "dotnet-test-explorer:Test project path" and write inside of it => "**/*Tests.csproj".
//Added "dotnet add package FluentAssertions" library to Catalog.UnitTests project. In our case this library will helps us compare existingItem and itemDto without code repeat.