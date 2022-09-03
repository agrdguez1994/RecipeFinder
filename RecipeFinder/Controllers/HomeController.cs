using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeFinder.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using System.Security.Principal;

namespace RecipeFinder.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Results(string value)
        {
            try
            {
                Recipes personObject = new Recipes();
                personObject.Meals = new List<Meal>();

                personObject =await ApiData.GetData(value, 0);

                ViewData["value"] = value;

                return View(personObject);

            }
            catch (Exception ex)
            {
                return NotFound();


            }

        }
        
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            try
            {

                Recipes personObject = new Recipes();
                personObject.Meals = new List<Meal>();

                personObject = await ApiData.GetData(id, 1);

                return View(personObject);

            }
            catch (Exception ex)
            {
                return NotFound();

            }
            
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}