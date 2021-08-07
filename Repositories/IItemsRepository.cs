using System;
using System.Collections.Generic;
using catalog.Entities;

namespace catalog.Repositories//Using namespace from folder's name
{
    public interface IItemsRepository//This Interface extracted from InMemItemsRepository then copy pasted to this file.
    {
        IEnumerable<Item> GetItems();
        Item GetItems(Guid id);
    }
}