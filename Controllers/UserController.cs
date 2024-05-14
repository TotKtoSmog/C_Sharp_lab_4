using C_Sharp_lab_4.DbContexts;
using C_Sharp_lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Threading.Tasks;

namespace C_Sharp_lab_4.Controllers
{
    public class UserController : Controller
    {
        private User _user = new User();
        private readonly ILogger<UserController> _logger;
        private readonly MyDbContext _context;
        public UserController(ILogger<UserController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index() => View();
        public IActionResult Registration() => View();
        public async Task<IActionResult> Аccount(User userModel)
            => View(await _context.Users.FindAsync(userModel.Id));
        public async Task<IActionResult> AllUsers()
            => View(await _context.Users.ToListAsync());
        public async Task<IActionResult> Login(User userModel)
        {
            ModelState.Remove("id");
            ModelState.Remove("fio");
            if (ModelState.IsValid && _context != null)
            {
                _user = await _context.Users.Where(u => u.Password == userModel.Password && u.Login == userModel.Login).SingleOrDefaultAsync();
                if (_user != null) return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _user.Id });
                ModelState.AddModelError("", "Неверные логин или пароль");
            }
            return View(userModel);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
