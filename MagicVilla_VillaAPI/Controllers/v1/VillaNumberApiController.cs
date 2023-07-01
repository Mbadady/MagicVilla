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

namespace MagicVilla_VillaAPI.Controllers.v1
{
    [Route("api/v{version:apiVersion}/VillaNumberApi")]
    [ApiController]
    [ApiVersion("1.0")]
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

        // GET: api/VillaNumberApi
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers(int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                //return await _db.VillaNumbers.ToListAsync();

                IEnumerable<VillaNumber> villaNumbers = await _villaNumber.GetAllVillaNumberAsync(includeProperties: "Villa", pageSize: pageSize, pageNumber: pageNumber);
            
                if (villaNumbers == null)
                {
                    return NotFound();
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumbers);

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

        // GET: api/VillaNumberApi/5

        [HttpGet("{id}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _villaNumber.GetVillaNumberAsync(u => u.VillaNo == id, includeProperties:"Villa");

                if (villaNumber == null)
                {
                    return NotFound();
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);

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

        // PUT: api/VillaNumberApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateVillaDTO)
        {
            try
            {

                if (id != updateVillaDTO.VillaNo || updateVillaDTO == null)
                {
                    return BadRequest();
                }

                //_db.Entry(villaNumber).State = EntityState.Modified;

                //_db.VillaNumbers.Update(villaNumber);

                

                if (await _dbVilla.GetAsync(u => u.Id == updateVillaDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is invalid");
                    return BadRequest(ModelState);
                }

                var villa = await _villaNumber.GetVillaNumberAsync(u => u.VillaNo == id);

                if (villa == null)
                {
                    return NotFound();
                }
                VillaNumber model = _mapper.Map<VillaNumber>(updateVillaDTO);

                await _villaNumber.UpdateAsync(model);
                //await _villaNumber.SaveAsync();

                _response.IsSuccess = true;
                _response.Result = _mapper.Map<VillaNumberUpdateDTO>(model);
                _response.StatusCode = HttpStatusCode.OK;

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

        // POST: api/VillaNumberApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "admin")]
        [HttpPost(Name = "CreateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody]VillaNumberCreateDTO villaNumberDTO)
        {
            try
            {
                if (villaNumberDTO == null)
                {
                    return BadRequest(villaNumberDTO);

                }

                var villaNumberId = await _villaNumber.GetVillaNumberAsync(u => u.VillaNo == villaNumberDTO.VillaNo);

                if (villaNumberId != null)
                {
                    //return Conflict();
                    ModelState.AddModelError("ErrorMessages", "Villa number already Exists");
                    return BadRequest(ModelState);
                }

                if(await _dbVilla.GetAsync(u => u.Id == villaNumberDTO.VillaID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Villa ID is invalid");
                    return BadRequest(ModelState);
                }
                var model = _mapper.Map<VillaNumber>(villaNumberDTO);
                //_db.VillaNumbers.Add(model);

                await _villaNumber.CreateAsync(model);

                await _villaNumber.SaveAsync();

                _response.StatusCode = HttpStatusCode.Created;
                _response.Result = _mapper.Map<VillaNumberCreateDTO>(model);

                return CreatedAtAction("GetVillaNumber", new { id = model.VillaNo }, _response);
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

        // DELETE: api/VillaNumberApi/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await _villaNumber.GetVillaNumberAsync(u => u.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }

                //_db.VillaNumbers.Remove(villaNumber);
                //await _db.SaveChangesAsync();
                await _villaNumber.RemoveAsync(villaNumber);
                await _villaNumber.SaveAsync();

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
