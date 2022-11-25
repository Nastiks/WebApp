using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplic.Data;
using WebApplic.Models;

namespace WebApplic.Areas
{
    public class TableOutputModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IEnumerable<ApplicationUser> Users { get; set; }
        public TableOutputModel(ApplicationDbContext db)
        {
            _context = db;
        }
        public void OnGet()
        {
            Users = _context.ApplicationUser;
        }
    }
}
