using Project1.IRepositories;
using Project1.Models.MainContext;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainDbContext _Context;
        private Dictionary<string, object> repositories;

        public UnitOfWork(MainDbContext mainDbContext)
        {
            this._Context = mainDbContext;
        }


        public int Save(bool? commit = true)
        {
            return this._Context.SaveChanges(commit.Value);
        }

        public IGenericRepository<TDbEntity> Repository<TDbEntity>() where TDbEntity : _BaseEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<string, object>();
            }

            var typeToInstantiate = typeof(_GenericRepository<TDbEntity>).Assembly.GetExportedTypes()
                .FirstOrDefault(t => t.BaseType == typeof(_GenericRepository<TDbEntity>)) ?? typeof(_GenericRepository<TDbEntity>);

            var type = typeof(TDbEntity).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryInstance = Activator.CreateInstance(typeToInstantiate, this._Context);
                repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<TDbEntity>)repositories[type];
        }
    }
}
