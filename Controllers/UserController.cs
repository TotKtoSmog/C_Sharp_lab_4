using C_Sharp_lab_4.DbContexts;
using C_Sharp_lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace C_Sharp_lab_4.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly MyDbContext _context;
        public UserController(ILogger<UserController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        public IActionResult Index() => View();
        public IActionResult Registration() => View();
        public async Task<IActionResult> AllUsers()
        {
            return View(await _context.Users.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
