using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
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

        // =========================
        // MÉTODO PARA VALIDAR ADMIN
        // =========================
        private bool EsAdmin()
        {
            // Por ahora usamos el rol "Docente" como administrador
            return HttpContext.Session.GetString("Rol") == "Docente";
        }

        // =========================
        // MENÚ PRINCIPAL ADMIN
        // =========================
        public IActionResult Index()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // =========================
        // LISTAR ALUMNOS
        // =========================
        public IActionResult Alumnos()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            var alumnos = _context.Alumnos
                .AsNoTracking()
                .OrderBy(a => a.NombreCompleto)
                .ToList();

            return View(alumnos);
        }

        // =========================
        // CREAR ALUMNO - GET
        // =========================
        public IActionResult CrearAlumno()
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // =========================
        // CREAR ALUMNO - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearAlumno(Alumno alumno)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(alumno);
            }

            // Guardar alumno
            _context.Alumnos.Add(alumno);
            _context.SaveChanges();

            // Crear notas en 0 para todas las materias existentes
            var materias = _context.Materias.ToList();

            foreach (var materia in materias)
            {
                var nota = new Nota
                {
                    AlumnoId = alumno.Id,
                    MateriaId = materia.Id,
                    Parcial1 = 0,
                    Parcial2 = 0,
                    NotaFinal = 0
                };
                _context.Notas.Add(nota);
            }

            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno creado correctamente.";
            return RedirectToAction(nameof(Alumnos));
        }

        // =========================
        // EDITAR ALUMNO - GET
        // =========================
        public IActionResult EditarAlumno(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos.Find(id);
            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // =========================
        // EDITAR ALUMNO - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarAlumno(Alumno alumno)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(alumno);
            }

            _context.Update(alumno);
            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno actualizado correctamente.";
            return RedirectToAction(nameof(Alumnos));
        }

        // =========================
        // ELIMINAR ALUMNO - GET
        // =========================
        public IActionResult EliminarAlumno(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // =========================
        // ELIMINAR ALUMNO - POST
        // =========================
        [HttpPost, ActionName("EliminarAlumno")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarAlumnoConfirmado(int id)
        {
            if (!EsAdmin())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos
                .Include(a => a.Notas)
                .FirstOrDefault(a => a.Id == id);

            if (alumno == null)
                return NotFound();

            // Primero eliminar notas relacionadas
            if (alumno.Notas != null)
            {
                _context.Notas.RemoveRange(alumno.Notas);
            }

            _context.Alumnos.Remove(alumno);
            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno eliminado correctamente.";
            return RedirectToAction(nameof(Alumnos));
        }
    }

}
