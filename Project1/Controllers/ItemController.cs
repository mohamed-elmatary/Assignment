using Microsoft.AspNetCore.Mvc;
using Project1.IServices;
using Project1.Models.Models;
using Project1.Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Project1.Controllers
{

    public class ItemController : _BaseController<Item, ItemDTO>
    {
        private readonly IItemService _itemServicee;

        public ItemController(IItemService itemServicee) : base(itemServicee)
        {
            this._itemServicee = itemServicee;
        }


    }
}
