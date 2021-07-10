using Project1.Models.Models;
using Project1.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.IRepositories
{
    public interface IUnitOfWork
    {

        int Save(bool? commit = true);
        IGenericRepository<TDbEntity> Repository<TDbEntity>() where TDbEntity : _BaseEntity;
    }
}
