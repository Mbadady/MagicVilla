using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public VillaNumberController(IMapper mapper, IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        // GET: /<controller>/
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();

            // This is how to get the token from the session and passing it to the methods
            //HttpContext.Session.GetString(SD._sessionToken)

            var response = await _villaNumberService.GetAllVillaNumberAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));

            if (response.IsSuccess && response != null)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberCreateVM = new();


            var response = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));

            if (response.IsSuccess && response != null)
            {
                villaNumberCreateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

            }
            return View(villaNumberCreateVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {

            var response = await _villaNumberService.CreateVillaNumberAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD._sessionToken));

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            else
            {
                if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }

            // Populate the model again
            var resp = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));

            if (resp.IsSuccess && resp != null)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            VillaNumberUpdateVM villaNumberUpdateVM = new();

            var response = await _villaNumberService.GetVillaNumberAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD._sessionToken));

            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberUpdateDTO>(model);

            }

            response = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));
            if (response != null && response.IsSuccess)
            {
                villaNumberUpdateVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                return View(villaNumberUpdateVM);
            }

            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {

            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateVillaNumberAsync<APIResponse>(model.VillaNumber, HttpContext.Session.GetString(SD._sessionToken));

                if(response.IsSuccess && response != null)
                {
                    return RedirectToAction(nameof(IndexVillaNumber));

                }
                else
                {
                    if (response.ErrorMessages.Count > 0)
                    {
                        ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                    }
                }
            }
            

            // Populate the model again
            var resp = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));
            if (resp != null && resp.IsSuccess)
            {
                model.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(resp.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            VillaNumberVM villaNumberVM = new();

            var response = await _villaNumberService.GetVillaNumberAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD._sessionToken));

            if (response != null && response.IsSuccess)
            {
                VillaNumberDTO model = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                villaNumberVM.VillaNumber = model;

            }

            response = await _villaService.GetAllVillaAsync<APIResponse>(HttpContext.Session.GetString(SD._sessionToken));
            if (response != null && response.IsSuccess)
            {
                villaNumberVM.VillaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result))
                    .Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });

                return View(villaNumberVM);
            }

            return NotFound();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberVM model)
        {

            var response = await _villaNumberService.DeleteVillaNumberAsync<APIResponse>(model.VillaNumber.VillaNo, HttpContext.Session.GetString(SD._sessionToken));

            if (response.IsSuccess && response != null)
            {

                return RedirectToAction(nameof(IndexVillaNumber));
            }
            return View(model);
        }

    }
}

