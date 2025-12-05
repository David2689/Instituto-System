using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsDocenteOAdmin()
        {
            var rol = HttpContext.Session.GetString("Rol");
            return rol == "Docente" || rol == "Admin";
        }

        // GET: /Admin
        public IActionResult Index()
        {
            if (!EsDocenteOAdmin())
                return RedirectToAction("Login", "Account");

            // 👉 Usa Views/Admin/Index.cshtml
            return View();
        }

        // GET: /Admin/Alumnos
        [HttpGet]
        public IActionResult Alumnos()
        {
            if (!EsDocenteOAdmin())
                return RedirectToAction("Login", "Account");

            var alumnos = _context.Alumnos
                .OrderBy(a => a.NombreCompleto)
                .ToList();

            // 👉 Usa Views/Admin/Alumnos.cshtml
            return View(alumnos);
        }
    }
}
