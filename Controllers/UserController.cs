using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplic.Data;
using WebApplic.Models;

namespace WebApplic.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = db;
        }


        public IActionResult Index()
        {
            Dictionary<ApplicationUser, bool> userList = new();
            foreach (var item in _context.ApplicationUser)
            {
                userList.Add(item, false);
            }
            return View(userList);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmedAsync(string userList)
        {
            string[] users = userList.Split(',');
            foreach (var item in users)
            {
                Console.WriteLine(item);
            }
            foreach (var item in users)
            {
                var user = await _userManager.FindByIdAsync(item);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    Program.LogoutCache[user.UserName] = true;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BlockAsync(string userList)
        {
            string[] users = userList.Split(',');
            foreach (var item in users)
            {
                Console.WriteLine(item);
            }
            foreach (var item in users)
            {
                var user = await _userManager.FindByIdAsync(item);
                if (user != null)
                {
                    user.Blocked = true;
                    await _userManager.UpdateAsync(user);
                    Program.LogoutCache[user.UserName] = true;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnblockAsync(string userList)
        {
            string[] users = userList.Split(',');
            foreach (var item in users)
            {
                Console.WriteLine(item);
            }
            foreach (var item in users)
            {
                var user = await _userManager.FindByIdAsync(item);
                if (user != null)
                {
                    user.Blocked = false;
                    await _userManager.UpdateAsync(user);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            else
            {
                return View();
            }
        }
    }
}