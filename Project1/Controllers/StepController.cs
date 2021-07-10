using Microsoft.AspNetCore.Mvc;
using Project1.IServices;
using Project1.Models.Models;
using Project1.Models.ProjectionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Project1.Controllers
{

    public class StepController : _BaseController<Step, StepDTO>
    {
        private readonly IStepService _stepService;

        public StepController(IStepService stepService) : base(stepService)
        {
            _stepService = stepService;
        }
    }
}
