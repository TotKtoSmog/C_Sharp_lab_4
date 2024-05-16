using C_Sharp_lab_4.DbContexts;
using C_Sharp_lab_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace C_Sharp_lab_4.Controllers
{
    public class UserController : Controller
    {
        private const string KeyLogin = "login";
        private const string KeyId = "id";
        private const string KeyFIO = "fio";
        private const string KeyPassword = "Password";

        private readonly ILogger<UserController> _logger;
        private readonly MyDbContext _context;
        public UserController(ILogger<UserController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index() => View();
        public async Task<IActionResult> Registration(RegistModel RM)
        {
            User _user = await AutoLogin();

            if (_user != null) return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _user.Id });
            ModelState.Remove("id");
            if(ModelState.IsValid && _context != null)
            {
                if(_context.Users.Any(u => u.Login == RM.Login)) return View();

                await _context.Users.AddAsync(RM);
                await _context.SaveChangesAsync();

                _user = await _context.Users.Where(u => u.Password == RM.Password && u.Login == RM.Login).SingleOrDefaultAsync();
                if(_user != null)
                {
                    SaveCooKie(_user);
                    return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _user.Id });
                }
            }
            return View(RM);
        }
        public async Task<IActionResult> Аccount(int id)
        {
            var _id = Request.Cookies[KeyId];
            if (_id == null) return RedirectToAction("Login");
            if(id.ToString() != _id) return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _id });

            User _user = await _context.Users.FindAsync(int.Parse(_id));
            ViewData["name"] = _user.FIO;

            var message = (from msg in _context.Message.ToList()
                           where msg.Id_Sender == _user.Id
                           select msg).ToList().Select(msg => new MessageModel
                           {
                                Id = msg.Id,
                                Recipient = _context.Users.ToList().First(u => u.Id == msg.Id_Recipient).Login,
                                Hedder = msg.Hedder,
                                TextMessage = msg.TextMessage,
                                Date = msg.DateDispatch,
                                Status = msg.Status
                           }).ToList().OrderByDescending(m => m.Date);

            return View(message);
        }
        public async Task<IActionResult> AllUsers()
            => View(await _context.Users.ToListAsync());
        public async Task<IActionResult> Login(User userModel)
        {
            var login = Request.Cookies[KeyLogin];
            var password = Request.Cookies[KeyPassword];
            User _user = await AutoLogin();
            if (_user != null) return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _user.Id });

            ModelState.Remove("id");
            ModelState.Remove("fio");

            if (ModelState.IsValid && _context != null)
            {
                _user = await _context.Users.Where(u => u.Password == userModel.Password && u.Login == userModel.Login).SingleOrDefaultAsync();
                if (_user != null)
                {
                    SaveCooKie(_user);
                    return RedirectToAction("Аccount", new { controller = "User", action = "Аccount", id = _user.Id });
                }
                ModelState.AddModelError("", "Неверные логин или пароль");
            }
            return View(userModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult LogOut()
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Append(KeyLogin, "", options);
            Response.Cookies.Append(KeyPassword, "", options);
            Response.Cookies.Append(KeyId, "", options);
            Response.Cookies.Append(KeyFIO, "", options);
            return RedirectToAction("Login");
        }
        public void SaveCooKie(User _user)
        {
            CookieOptions cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(KeyLogin, _user.Login, cookieOptions);
            Response.Cookies.Append(KeyPassword, _user.Password, cookieOptions);
            Response.Cookies.Append(KeyId, _user.Id.ToString(), cookieOptions);
            Response.Cookies.Append(KeyFIO, _user.FIO, cookieOptions);
        }
        private async Task<User> AutoLogin()
        {
            var login = Request.Cookies[KeyLogin];
            var password = Request.Cookies[KeyPassword];
            if(login == null || password == null) return null;
            User _user = await _context.Users.Where(u => u.Password == password && u.Login == login).SingleOrDefaultAsync();
            if (_user == null) return null;
            return _user;
        }
        public IActionResult Error() 
            => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            var message = _context.Message.Find(id);
            if (message == null)
            {
                return NotFound();
            }

            message.Status = false;
            _context.Message.Update(message);
            _context.SaveChanges();

            return NoContent();
        }

    }
}
