using Microsoft.AspNetCore.Mvc;
using WebApplic.Data;
using WebApplic.Models;

namespace WebApplic.Controllers
{
    public class UserTableController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly Dictionary<ApplicationUser, bool> userList = new();

        public UserTableController(ApplicationDbContext db)
        {
            _db = db;
            
            foreach (var item in _db.ApplicationUser)
            {
                userList.Add(item, false);
            }
        }

        public IActionResult Index()
        {
            return View(userList);
        }
    }
}
