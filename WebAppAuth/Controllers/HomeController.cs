using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebAppAuth.Models;

namespace WebAppAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnrurl)
        {
            ViewBag.ReturnUrl = returnrurl; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnrurl)
        {
            if((username == "Admin") && (password == "Admin"))
            {
                var claims = new List<Claim> //утверждения позваляют хранить приватные данные (не обращяться в базу данных)
                {
                    new Claim(ClaimTypes.Name, username),
                };

                var claimsidentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsidentity));

                return Redirect(returnrurl == null ? "/Secured" : returnrurl); //на какой ссылке ты был на ту и попадешь
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}