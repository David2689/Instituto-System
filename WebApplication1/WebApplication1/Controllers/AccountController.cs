using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string password)
        {
            // =============== 1) LOGIN ADMIN =================
            // Ejemplo: usuario: admin  |  contraseña: admin123
            if (usuario == "admin" && password == "admin123")
            {
                HttpContext.Session.SetString("Rol", "Admin");
                HttpContext.Session.SetString("NombreUsuario", "Administrador INU");

                // Lo mandamos a donde tú quieras que empiece el admin
                // Ejemplo al panel administrativo:
                return RedirectToAction("Index", "Admin");
            }

            // =============== 2) LOGIN DOCENTE ================
            // Ejemplo: usuario: docente  |  contraseña: admin123
            if (usuario == "docente" && password == "admin123")
            {
                HttpContext.Session.SetString("Rol", "Docente");
                HttpContext.Session.SetString("NombreUsuario", "Docente del INU");

                // Lo mandamos al panel de docente
                return RedirectToAction("Alumnos", "Docente");
            }

            // =============== 3) LOGIN ALUMNO =================
            // Carné + contraseña fija "12345"
            if (!string.IsNullOrWhiteSpace(usuario))
            {
                var alumno = _context.Alumnos.FirstOrDefault(a => a.Carne == usuario);

                if (alumno != null && password == "12345")
                {
                    HttpContext.Session.SetString("Rol", "Alumno");
                    HttpContext.Session.SetInt32("AlumnoId", alumno.Id);
                    HttpContext.Session.SetString("NombreUsuario", alumno.NombreCompleto);

                    // Lo mandamos a ver sus notas
                    return RedirectToAction("Index", "Notas", new { idAlumno = alumno.Id });
                }
            }

            // =============== 4) SI NADA COINCIDE =============
            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
