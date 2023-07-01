using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_VillaAPI.DataStore;
using Microsoft.AspNetCore.JsonPatch;
using MagicVilla_VillaAPI.Logging;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MagicVilla_VillaAPI.Repository.IRepository;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace MagicVilla_VillaAPI.Controllers.v1
{

    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/VillaApi")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaApiController : ControllerBase
    {
        private readonly ILogger<VillaApiController> _logger;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaApiController(ILogger<VillaApiController> logger, IVillaRepository dbVilla, IMapper mapper)
        {
            _logger = logger;
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();
        }

        // Custom Logging
        //private readonly ILogging _logging;

        //public VillaApiController(ILogging logging)
        //{
        //    _logging = logging;
        //}

        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<APIResponse>> GetVillas([FromQuery]int? occupancy,
            [FromQuery]string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {


                _logger.LogInformation("Getting all Villas");

                // Custom logging
                //_logging.Log("Getting All Villas", "");

                // Using a data store instead of a db
                //return Ok(DataStore.DataStore.VillaStore);

                // For database

                IEnumerable<Villa> villaList;

                if(occupancy > 0)
                {
                    villaList = await _dbVilla.GetAllAsync(u => u.Occupancy == occupancy, pageSize: pageSize, pageNumber: pageNumber);
                }

                else
                {
                    villaList = await _dbVilla.GetAllAsync();
                }

                if (!string.IsNullOrEmpty(search))
                {
                    villaList = villaList.Where(u => u.Name.ToLower().Contains(search) || u.Amenity.ToLower().Contains(search));
                }

                Pagination pagination = new Pagination() { PageNumber = pageNumber, PageSize = pageSize };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
                _response.StatusCode = HttpStatusCode.OK;
                //AutoMapper 
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };

                return _response;
            }

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        
        //[ProducesResponseType(200, Type =typeof(VillaDTO))]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {

                    _logger.LogError("Get Villa Error with Id " + id);

                    // Custom logging
                    //_logging.Log("Get Villa Error with Id " + id, "error");
                    return BadRequest();
                }
                // For Datastore
                //var villa = DataStore.DataStore.VillaStore.FirstOrDefault(u => u.Id == id);
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                //_response.ErrorMessages = new List<string>()
                //{
                //    ex.Message
                //};

                //OR

                _response.ErrorMessages.Add(ex.Message);

                return _response;

            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDTO createDTO)
        {
            try
            {
                if (await _dbVilla.GetAsync(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa already Exists");

                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                //if(villaDTO.Id > 0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}

                // Giving Id when we were making use of data store
                //villaDTO.Id = DataStore.DataStore.VillaStore.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

                //DataStore.DataStore.VillaStore.Add(villaDTO);

                Villa model = _mapper.Map<Villa>(createDTO);
                //Villa model = new Villa()
                //{
                //    Amenity = createDTO.Amenity,
                //    Occupancy = createDTO.Occupancy,
                //    Name = createDTO.Name,
                //    Details = createDTO.Details,
                //    ImageUrl = createDTO.ImageUrl,
                //    Rate = createDTO.Rate,
                //    Sqft = createDTO.Sqft,
                //};
                await _dbVilla.CreateAsync(model);
                await _dbVilla.SaveAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaDTO>(model);
                _response.IsSuccess = true;

                //return Ok(_response);
                //return Ok(villaDTO);
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };
                return _response;
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {


                if (id == 0)
                {
                    return BadRequest();
                }

                //var villa = DataStore.DataStore.VillaStore.Find(u => u.Id == id);

                var villa = await _dbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                await _dbVilla.RemoveAsync(villa);

                await _dbVilla.SaveAsync();

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };

                return _response;
            }
        }


        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDTO)
        {

            try
            {


                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }

                

                Villa model = _mapper.Map<Villa>(updateDTO);

                await _dbVilla.UpdateAsync(model);

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaUpdateDTO>(model);
                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };

                return _response;

            }

        }


        [Authorize(Roles = "admin")]
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                //var villa = DataStore.DataStore.VillaStore.FirstOrDefault(u => u.Id == id);
                var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);

                if (villa == null)
                {
                    return NotFound();
                }

                VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);
         

                patchDto.ApplyTo(villaDTO, ModelState);

                Villa model = _mapper.Map<Villa>(villaDTO);
               

                await _dbVilla.UpdateAsync(model);
                await _dbVilla.SaveAsync();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);

            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message
                };

                return _response;
            }
        }
    }
}
