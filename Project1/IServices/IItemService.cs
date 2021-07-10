using Project1.Models.Models;
using Project1.Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.IServices
{
    public interface IItemService : IBusinessService<Item, ItemDTO>
    {
        IEnumerable<ItemDTO> GetStepItems(int? StepId);

    }
}
