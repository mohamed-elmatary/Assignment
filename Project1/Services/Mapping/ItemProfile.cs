using AutoMapper;
using Project1.Models.Models;
using Project1.Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Services.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Step, StepDTO>();
            CreateMap<StepDTO, Step>();
        }
    }
}
