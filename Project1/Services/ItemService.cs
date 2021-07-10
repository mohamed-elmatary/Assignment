using AutoMapper;
using AutoMapper.QueryableExtensions;
using Project1.IRepositories;
using Project1.IServices;
using Project1.Models.Models;
using Project1.Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Services
{
    public class ItemService : BusinessService<Item, ItemDTO>, IItemService
    {
        public ItemService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {

        }
    }
}
