using C_Sharp_lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace C_Sharp_lab_4.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger) => _logger = logger;
        
        public IActionResult Index() => View();
        public IActionResult Registration() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
