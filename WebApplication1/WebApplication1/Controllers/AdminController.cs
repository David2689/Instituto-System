using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAdmin()
        {
            // De momento usamos el mismo rol "Docente" como administrador
            return HttpContext.Session.GetString("Rol") == "Docente";
        }

        public IActionResult Index()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}

