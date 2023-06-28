using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicVilla_VillaAPI.DataStore;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository.IRepository;
using AutoMapper;
using MagicVilla_VillaAPI.Model.DTO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace MagicVilla_VillaAPI.Controllers.v2
{
    [Route("api/v{version:apiVersion}/VillaNumberApi")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberApiController : ControllerBase
    {

        private readonly IVillaNumberRepository _villaNumber;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _dbVilla;
        protected readonly APIResponse _response;


        public VillaNumberApiController(IVillaNumberRepository villaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _mapper = mapper;
            _dbVilla = dbVilla;
            _villaNumber = villaNumber;
            _response = new();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<string> Get()
        {
            return new string[]
            {
                "Value1",
                "Value2"
            };
        }


    }
}
