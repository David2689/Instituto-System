using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MatriculaController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ==========================
        //  VALIDACIÓN DE ROL
        //  (Docente o Admin)
        // ==========================
        private bool EsDocenteOAdmin()
        {
            var rol = HttpContext.Session.GetString("Rol");
            return rol == "Docente" || rol == "Admin";
        }

        // ==========================
        //  FORMULARIO PÚBLICO
        //  GET: /Matricula/Crear
        // ==========================
        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        // ==========================
        //  GUARDAR MATRÍCULA
        //  POST: /Matricula/Crear
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Matricula model, IFormFile? CertificadoArchivo)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Guardar archivo si se subió alguno
            if (CertificadoArchivo != null && CertificadoArchivo.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CertificadoArchivo.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    CertificadoArchivo.CopyTo(stream);
                }

                model.CertificadoRuta = "/uploads/" + fileName;
            }

            _context.Matriculas.Add(model);
            _context.SaveChanges();

            TempData["Mensaje"] = "La inscripción se registró correctamente.";
            return RedirectToAction(nameof(Confirmacion));
        }

        // ==========================
        //  PANTALLA DE CONFIRMACIÓN
        //  GET: /Matricula/Confirmacion
        // ==========================
        [HttpGet]
        public IActionResult Confirmacion()
        {
            return View();
        }

        // ==========================
        //  LISTADO (SOLO DOCENTE/ADMIN)
        //  GET: /Matricula/Lista
        // ==========================
        [HttpGet]
        public IActionResult Lista()
        {
            // Si NO es Docente ni Admin → Login
            if (!EsDocenteOAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            var matriculas = _context.Matriculas
                .OrderBy(m => m.NombreCompleto)
                .ToList();

            return View(matriculas);
        }
    }
}
