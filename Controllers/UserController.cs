using C_Sharp_lab_4.DbContexts;
using C_Sharp_lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            => View(await _context.Users.ToListAsync());
        public async Task<IActionResult> Аccount(string Login, string Password)
        {
            User _user = await _context.Users.Where(u => u.Login == Login && u.Password == Password).SingleOrDefaultAsync();

            if (_user == null) return View("Index");
            return View(_user);
        } 
           

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
