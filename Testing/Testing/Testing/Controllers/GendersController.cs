using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testing.DomainModels;
using Testing.Repositories;

namespace Testing.Controllers
{

    [ApiController]
    public class GendersController : Controller
    {
        private readonly IStydentRepository stydentRepository;
        private readonly IMapper mapper;
        public GendersController(IStydentRepository stydentRepository,IMapper mapper)
        {
            this.stydentRepository = stydentRepository;
            this.mapper = mapper;
        }
        
        [HttpGet]
        [Route("[controller]")]


        public async Task<IActionResult> GetAllGenders()
        {
            var genderList = await stydentRepository.GetGendersAsync();
            if(genderList==null || !genderList.Any())
            {
                return NotFound();
            }
            return Ok(mapper.Map<List<Gender>>(genderList));
        }
    }
}
