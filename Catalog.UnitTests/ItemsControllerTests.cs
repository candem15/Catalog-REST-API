using System;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
//If you are getting red underlines after rename this file to "ItemsControllerTests" press f1 and restart OmniSharp.
namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        [Fact] //This attritibute used for declare that one of the methods in this class is test method.
        public async Task GetItemsAsync_WithUnexistingItem_ReturnsNotFound() //Naming unit tests is important because it helps to identify what is this test supposed to do. In this case we are using "following" method for naming unit tests => "UnitofWork_StateUnderTest_ExpectedBehavior".
        {
            //Arrange =>  In this section we set up everything to be ready to execute the test.
            
            var repositoryStub = new Mock<IItemsRepository>(); //We are using fake respository(Stub) for unit test because we dont want to test actual repository but only methods inside ItemsController.
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())) //In here doesn't matter what's the item that we are going to be passing because we are testing unexisted item.
                .ReturnsAsync((Item)null); //And we cast this into the item entity because Mock get confused. 
            
            var loggerStub = new Mock<ILogger<ItemsController>>();

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            
            //Act => In this section we execute the test.

            var result = await controller.GetItemAsync(Guid.NewGuid()); //Like we said it is not important what guid we pass because we are testing "return NotFound()" action.
            
            //Assert => In this section we verify whatever needs to be verified about the execution of the unit test.
        
            Assert.IsType<NotFoundResult>(result.Result); //"Result" property inside result represents the actual result that actual type of result we got, which in our case, it should be "Not Found".
        }
    }
}
//There is extension to visualize the overall status of our test suite. Like, which case is passing, which words are failing and so on. To do that we added ".NET Core Test Explorer" extension. Then click gear icon and go extension settings, switch to workspace section from user and find "dotnet-test-explorer:Test project path" and write inside of it => "**/*Tests.csproj".