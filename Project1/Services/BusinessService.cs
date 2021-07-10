using AutoMapper;
using AutoMapper.QueryableExtensions;
using Project1.Extensions;
using Project1.IRepositories;
using Project1.IServices;
using Project1.Models.MainContext.Extensions;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Services
{
    public abstract class BusinessService : IBusinessService
    {

        protected readonly IUnitOfWork _UnitOfWork;
        protected readonly IMapper _Mapper;

        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _UnitOfWork = unitOfWork;
            _Mapper = mapper;
        }
    }
    public abstract class BusinessService<TDbEntity, TDetailsDTO> : BusinessService, IBusinessService<TDbEntity, TDetailsDTO>
             where TDbEntity : _BaseEntity
             where TDetailsDTO : class
    {
        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public virtual IQueryable<T> GetAll<T>(SearchModel searchModel, bool WithTracking = true)
        {
            IQueryable query = this._UnitOfWork.Repository<TDbEntity>().GetAll(WithTracking).DynamicSearch(searchModel).ToCustomPagedList(searchModel.take, searchModel.skip);
            if (typeof(TDbEntity) == typeof(T))
                return query.Cast<T>();
            else
                return query.ProjectTo<T>(_Mapper.ConfigurationProvider);
        }

        public virtual TDetailsDTO GetDetails(object Id, bool WithTracking = true)
        {
            if (Id == null) return null;
            var Mapping = _Mapper.ConfigurationProvider.FindTypeMapFor(typeof(TDbEntity), typeof(TDetailsDTO));
            if (Mapping == null)
            {
                Mapping = _Mapper.ConfigurationProvider.ResolveTypeMap(typeof(TDbEntity), typeof(TDetailsDTO));
            }
            var EntityObject = this._UnitOfWork.Repository<TDbEntity>().GetById(Id);
            if (typeof(TDbEntity) == typeof(TDetailsDTO))
                return EntityObject as TDetailsDTO;
            else
                return _Mapper.Map(EntityObject, typeof(TDbEntity), typeof(TDetailsDTO)) as TDetailsDTO;
        }

        public virtual IEnumerable<TDetailsDTO> Insert(IEnumerable<TDetailsDTO> entities)
        {
            var TDbEntities = entities.AsQueryable().ProjectTo<TDbEntity>(_Mapper.ConfigurationProvider).ToList();
            var ToBereturned = this._UnitOfWork.Repository<TDbEntity>().Insert(TDbEntities);
            return _Mapper.Map(ToBereturned, typeof(IEnumerable<TDbEntity>), typeof(IEnumerable<TDetailsDTO>)) as IEnumerable<TDetailsDTO>;
        }
        public virtual IEnumerable<object> Delete(IEnumerable<object> Ids)
        {
            var DeletedRecords = this._UnitOfWork.Repository<TDbEntity>().Delete(Ids);
            return DeletedRecords.Count() == Ids.Count() ? DeletedRecords : null;
        }
        public virtual IEnumerable<TDetailsDTO> Update(IEnumerable<TDetailsDTO> Entities)
        {
            int RecordsUpdated = 0;
            foreach (var Entity in Entities)
            {
                var PrimaryKeysValues = this._UnitOfWork.Repository<TDbEntity>().GetKey<TDbEntity>(_Mapper.Map(Entity, typeof(TDetailsDTO), typeof(TDbEntity)) as TDbEntity);
                var OldEntity = this._UnitOfWork.Repository<TDbEntity>().Find(PrimaryKeysValues);
                object MappedEntity = _Mapper.Map(Entity, OldEntity, typeof(TDetailsDTO), typeof(TDbEntity));
                this._UnitOfWork.Repository<TDbEntity>().Update(MappedEntity as TDbEntity);
                RecordsUpdated++;
            }
            return RecordsUpdated == Entities.Count() ? Entities : null;
        }
    }
}
