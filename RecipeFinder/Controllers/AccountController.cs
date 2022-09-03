using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RecipeFinder.Controllers
{
    public class AccountController : Controller
    {
  
    
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            try
            {
                string url = Url.Action("GoogleResponse", "Account");
                return new ChallengeResult(
                    GoogleDefaults.AuthenticationScheme,
                    new AuthenticationProperties
                    {
                        RedirectUri = url
                    });

            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> GoogleResponse()
        {
            
            var authenticateResult = await HttpContext.AuthenticateAsync("External");
            if (!authenticateResult.Succeeded)
                return BadRequest(); 
            if (authenticateResult.Principal.Identities.ToList()[0].AuthenticationType.ToLower() == "google")
            {
                if (authenticateResult.Principal != null)
                {
                    var googleAccountId = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var claimsIdentity = new ClaimsIdentity("Application");
                    if (authenticateResult.Principal != null)
                    {
                        var details = authenticateResult.Principal.Claims.ToList();
                        claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier));// Full Name Of The User
                        claimsIdentity.AddClaim(authenticateResult.Principal.FindFirst(ClaimTypes.Email)); // Email Address of The User
                        await HttpContext.SignInAsync("Application", new ClaimsPrincipal(claimsIdentity));
                       
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> SignOutFromGoogleLogin()
        {
            
            if (HttpContext.Request.Cookies.Count > 0)
            {
                var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
                foreach (var cookie in siteCookies)
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }            
            await HttpContext.SignOutAsync("External");
            return RedirectToAction("Index", "Home");


        }
    }
}
