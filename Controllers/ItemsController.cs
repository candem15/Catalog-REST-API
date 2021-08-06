using System;
using System.Collections.Generic;
using catalog.Entities;
using catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace catalog.Controllers
{
    [ApiController]//This brings bunch of additional behavior for controller class that makes lifes easier.
    [Route("items")]
    public class ItemsController:ControllerBase //ControllerBase must be inherited for making from class to controller.
    {
        private readonly InMemItemsRepository repository; //In controller we need add "instance" of repository that we created for any action or operation.
        public ItemsController()//Constructor of this controller.
        {
            repository=new InMemItemsRepository();//This is work like "repository writer".
        }
        [HttpGet]// GET /items 
        public IEnumerable<Item> GetItems() //This will return all items in list "items".
        {
            var items=repository.GetItems();//Invokes GetItems method from Repositories folder for get all items and equalize it to variable then return.
            return items;
        }
        [HttpGet("{id}")]// GET /items/{id}
        public ActionResult<Item> GetItems(Guid id) //This will return one item in list "items".
        {
            var item=repository.GetItems(id);//Invokes GetItems method from Repositories folder for get one item and equalize it to variable then return.
            if(item==null)
            {
               return NotFound();//ActionResult type allowes us both return list item or NotFound function.
            }

            return item;
        }
    }
}