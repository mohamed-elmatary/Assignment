using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.IRepositories
{
    public interface IGenericRepository<TDbEntity> where TDbEntity : _BaseEntity
    {
        IQueryable<TDbEntity> GetAll(bool WithTracking = true);
        TDbEntity GetById(object Id);
        IEnumerable<TDbEntity> Insert(IEnumerable<TDbEntity> Entities);
        TDbEntity Insert(TDbEntity Entity);
        IEnumerable<object> Delete(IEnumerable<object> Ids);
        void Update(TDbEntity Entity);
        void UpdateRange(IEnumerable<TDbEntity> Entites);
        object[] GetKey<T>(T entity);
        object[] GetKeyNames<T>(T entity);
        TDbEntity Find(params object[] Ids);



    }
}
