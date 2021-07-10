using Microsoft.EntityFrameworkCore;
using Project1.IRepositories;
using Project1.Models.MainContext;
using Project1.Models.MainContext.Extensions;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Project1.Repositories
{

    public class _GenericRepository<TDbEntity> : IGenericRepository<TDbEntity> where TDbEntity : _BaseEntity
    {
        protected readonly MainDbContext _Context;
        public readonly DbSet<TDbEntity> _DbSet;


        public _GenericRepository(MainDbContext mainDbContext)
        {
            this._Context = mainDbContext;
            this._DbSet = this._Context.Set<TDbEntity>();

        }

        public virtual IQueryable<TDbEntity> GetAll(bool WithTracking = true)
        {
            if (WithTracking)
                return this._DbSet;
            else
                return this._DbSet.AsNoTracking();
        }

        public virtual TDbEntity GetById(object Id)
        {
            return this._DbSet.FindQuery(Id).FirstOrDefault();
        }

        private void SetProperty(object obj, string property, object value)
        {
            try
            {
                var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                    prop.SetValue(obj, value, null);
            }
            catch { }
        }



        public virtual IEnumerable<TDbEntity> Insert(IEnumerable<TDbEntity> Entities)
        {

            try
            {

                int RecordsInserted;
                for (int i = 0; i < Entities.Count(); i++)
                {
                    SetProperty(Entities.ElementAt(i), "CreatedOn", DateTimeOffset.Now);
                    this._DbSet.Add(Entities.ElementAt(i));
                    RecordsInserted = _Context.SaveChanges();
                }

                return Entities;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public virtual TDbEntity Insert(TDbEntity Entity)
        {
            try
            {
                SetProperty(Entity, "CreatedOn", DateTimeOffset.Now);
                int RecordsInserted;
                this._DbSet.Add(Entity);
                RecordsInserted = this._Context.SaveChanges();
                return Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public virtual IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            for (int i = 0; i < Ids.Count(); i++)
            {
                var ToBeRemoved = this.GetById(Ids.ElementAt(i));
                this._DbSet.Remove(ToBeRemoved);
            }
            this._Context.SaveChanges();
            return Ids;
        }


        public virtual void Update(TDbEntity Entity)
        {
            this._DbSet.Update(Entity);
            this._Context.SaveChanges();
        }

        public virtual void UpdateRange(IEnumerable<TDbEntity> Entites)
        {
            this._DbSet.UpdateRange(Entites);
            this._Context.SaveChanges();
        }

        public virtual object[] GetKey<T>(T entity)
        {
            var keyNames = _Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name);
            List<object> result = new List<object>();
            for (int i = 0; i < keyNames.Count(); i++)
            {
                result.Add(entity.GetType().GetProperty(keyNames.ElementAt(i)).GetValue(entity, null));
            }
            return result.ToArray<object>();
        }

        public virtual object[] GetKeyNames<T>(T entity)
        {
            return _Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).ToArray();
        }
        public virtual TDbEntity Find(params object[] Ids)
        {
            return this._Context.Find<TDbEntity>(Ids);
        }
    }
}

