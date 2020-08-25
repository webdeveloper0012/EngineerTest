using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EngineerTest.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public IList<AuthenticationScheme> ExternalLogins { get; set; }        
        public string ReturnUrl { get; set; }

        public async Task SetExternalLoginList(SignInManager<ApplicationUser> signInManager, string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }
    }
}
