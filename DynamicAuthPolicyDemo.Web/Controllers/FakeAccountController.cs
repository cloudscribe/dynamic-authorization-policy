using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DynamicAuthPolicyDemo.Web.Controllers
{
    public class FakeAccountController : Controller
    {
        public FakeAccountController()
        {

        }

        // GET: /Account/index
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string returnUrl = null)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            ViewData["Title"] = "Login";
            if(!string.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string userName = null, string returnUrl = null)
        {
            //fake login with roles to demonstrate role based menu filtering
            AuthenticationProperties authProperties = new AuthenticationProperties();
            ClaimsPrincipal user;
            switch(userName)
            {
                case "Administrator":
                    user = GetAdminClaimsPrincipal();
                    break;

                case "Member":
                    user = GetMemberClaimsPrincipal();
                    break;

                case "Minion":
                default:
                    user = GetMinionClaimsPrincipal();
                    break;
            }
            await HttpContext.SignInAsync("application", user, authProperties);

            if(!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");


            //return View("Index");
        }

        private ClaimsPrincipal GetAdminClaimsPrincipal()
        {
            var identity = new ClaimsIdentity("application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Administrator"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admins"));

            identity.AddClaim(new Claim("Team", "Management"));
            identity.AddClaim(new Claim("ClearanceCode", "alpha"));


            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal GetMemberClaimsPrincipal()
        {
            var identity = new ClaimsIdentity("application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "2"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Member"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Members"));
            identity.AddClaim(new Claim("Team", "Marketing"));
            identity.AddClaim(new Claim("ClearanceCode", "beta"));


            return new ClaimsPrincipal(identity);
        }

        private ClaimsPrincipal GetMinionClaimsPrincipal()
        {
            var identity = new ClaimsIdentity("application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "3"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "Minion"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Minions"));
            identity.AddClaim(new Claim("Team", "Kitchen"));
            

            return new ClaimsPrincipal(identity);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            //await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync("application");

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
