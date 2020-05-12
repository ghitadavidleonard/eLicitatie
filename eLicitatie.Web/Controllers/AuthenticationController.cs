using Common.Authentication;
using eLicitatie.Web.Extensions;
using eLicitatie.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using eLicitatie.Web.Extensions.Mappers;

namespace eLicitatie.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public class AuthenticationController : BaseController
    {
        public AuthenticationController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            var signInRequest = model.MapToRequest();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("signin", signInRequest);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "The e-mail address or password are incorrect.";

                return View();
            }

            SignInResponse signInResponse = await response.ContentAsTypeAsync<SignInResponse>();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, signInResponse.Email),
                new Claim(ClaimTypes.Role, signInResponse.Role.ToString()),
                new Claim("access_token", signInResponse.Token),
                new Claim(ClaimTypes.NameIdentifier, signInResponse.UserId.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authenticationProperties
                );

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow,
                    IsPersistent = false
                });

            return RedirectToAction(nameof(SignIn));
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            SignUpRequest registerRequest = model.MapToRequest();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("signup", registerRequest);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                string modelErrorKey = errorMessage.Contains("Email") ? "Email" : "";

                ModelState.AddModelError(modelErrorKey, await response.Content.ReadAsStringAsync());

                return View();
            }

            return RedirectToAction(nameof(SignIn));
        }
    }
}