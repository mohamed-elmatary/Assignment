using AutoMapper;
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
    public class StepService : BusinessService<Step, StepDTO>, IStepService
    {
        public StepService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public override IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            foreach (var id in Ids)
            {

                var removedItems = _UnitOfWork.Repository<Item>().GetAll().Where(s => s.StepId == Convert.ToInt32(id));
                if (removedItems != null && removedItems.Count() > 0)
                {
                    _UnitOfWork.Repository<Item>().Delete(removedItems);
                }

            }
            return base.Delete(Ids);
        }


    }
}
