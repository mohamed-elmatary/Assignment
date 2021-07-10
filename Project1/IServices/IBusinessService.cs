using Project1.Extensions;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.IServices
{
    public interface IBusinessService
    {
    }

    public interface IBusinessService<TDbEntity, TDetailsDTO> : IBusinessService
        where TDbEntity : _BaseEntity
    {
        IQueryable<T> GetAll<T>(SearchModel searchModel, bool WithTracking = true);
        TDetailsDTO GetDetails(object Id, bool WithTracking = true);
        IEnumerable<TDetailsDTO> Insert(IEnumerable<TDetailsDTO> entities);
        IEnumerable<object> Delete(IEnumerable<object> Ids);
        IEnumerable<TDetailsDTO> Update(IEnumerable<TDetailsDTO> Entities);

    }

}
