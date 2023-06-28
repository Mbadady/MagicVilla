using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {

        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        // GET: /<controller>/
        public async Task<IActionResult> IndexVilla()
        {

            List<VillaDTO> list = new();

            var response = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO model)
        {           

            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateVillaAsync<APIResponse>(model, HttpContext.Session.GetString(SD._sessionToken));
                if (response.IsSuccess && response != null)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
                
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villaId)
        {
            var response = await _villaService.GetVillaAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD._sessionToken));

            if (response != null && response.IsSuccess)
            {
               VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(_mapper.Map<VillaUpdateDTO>(model));
            }
           
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateVillaAsync<APIResponse>(model, HttpContext.Session.GetString(SD._sessionToken));

                if(response.IsSuccess && response != null)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villaId)
        {
            var response = await _villaService.GetVillaAsync<APIResponse>(villaId, HttpContext.Session.GetString(SD._sessionToken));

            if (response != null && response.IsSuccess)
            {
                VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {

            var response = await _villaService.DeleteVillaAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD._sessionToken));

            if (response.IsSuccess && response != null)
            {

                return RedirectToAction(nameof(IndexVilla));
            }
            return View(model);
        }

    }
}

