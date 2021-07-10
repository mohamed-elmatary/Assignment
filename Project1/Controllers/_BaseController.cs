using Microsoft.AspNetCore.Mvc;
using Project1.IServices;
using Project1.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    public class _BaseController : Controller
    {
        protected readonly IBusinessService _BusinessService;


        public _BaseController(IBusinessService businessService)
        {
            this._BusinessService = businessService;
        }
    }


    [Route("api/[controller]")]
    public class _BaseController<TDbEntity, TDetailsDTO> : Controller where TDbEntity : _BaseEntity
    {
        protected readonly IBusinessService<TDbEntity, TDetailsDTO> BusinessService;

        public _BaseController(IBusinessService<TDbEntity, TDetailsDTO> businessService)
        {
            this.BusinessService = businessService;
        }


        [HttpGet, Route("GetAllDetails")]
        public virtual IEnumerable<TDetailsDTO> GetAllDetails()
        {
            return this.BusinessService.GetAll<TDetailsDTO>(false);
        }


        [HttpGet, Route("GetById")]
        public virtual TDetailsDTO Get(string id)
        {
            return BusinessService.GetDetails(id, false);
        }


        [HttpPost, Route("Insert")]
        public virtual IEnumerable<TDetailsDTO> Post([FromBody] IEnumerable<TDetailsDTO> entities)
        {
            return BusinessService.Insert(entities);
        }


        [HttpPut, Route("Update")]
        public virtual IEnumerable<TDetailsDTO> Put([FromBody] IEnumerable<TDetailsDTO> entities)
        {
            return this.BusinessService.Update(entities);
        }


        [HttpDelete, Route("Delete")]
        public virtual IEnumerable<object> Delete([FromBody] object[] ids)
        {

            return this.BusinessService.Delete(ids);
        }
    }

}
