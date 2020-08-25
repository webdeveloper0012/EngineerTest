using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using EngineerTest.Models;
using System.Security.Claims;


namespace EngineerTest.Controllers
{
    [Authorize]
    public class signingoogleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;


        public signingoogleController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<LoginViewModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        [Route("signin-google")]
        public async Task< IActionResult> Index()
        {

            
            var info = await _signInManager.GetExternalLoginInfoAsync();
            
            if (info == null)
            {
              
                return View();
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                //return LocalRedirect(Url.GetLocalUrl(returnUrl));
                return RedirectToLocal("");

            }
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Movie");
            }
        }
    }
}