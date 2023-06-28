using System;
using System.Security.Claims;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
	public class AuthController : Controller
	{

		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

        public IActionResult Login()
		{

			LoginRequestDTO obj = new();

            return View(obj);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
		{

            APIResponse result = await _authService.LoginAsync<APIResponse>(loginRequestDTO);

            if(result.IsSuccess && result != null)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(result.Result));

                // To tell HttpContext that this user is logged in

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, model.User.Username));
                identity.AddClaim(new Claim(ClaimTypes.Role, model.User.Role));

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                // To Save the token to the cache

                HttpContext.Session.SetString(SD._sessionToken, model.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", result.ErrorMessages.FirstOrDefault());

                return View(loginRequestDTO);
            }
		}

        public IActionResult Register()
        {
            RegistrationDTO obj = new();

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationDTO registrationDTO)
        {

            APIResponse apiResponse = await _authService.RegisterAsync<APIResponse>(registrationDTO);

            if(apiResponse.IsSuccess && apiResponse != null)
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD._sessionToken, "");


            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

