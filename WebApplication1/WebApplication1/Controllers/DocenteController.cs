using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class DocenteController : Controller
    {
        private readonly AppDbContext _context;

        public DocenteController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // MÉTODO PARA VER SI ES DOCENTE
        // ==============================
        private bool EsDocente()
        {
            return HttpContext.Session.GetString("Rol") == "Docente";
        }

        // ==============================
        // ALUMNOS - LISTAR
        // ==============================

        // GET: /Docente/Alumnos
        public IActionResult Alumnos()
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var alumnos = _context.Alumnos
                .AsNoTracking()
                .OrderBy(a => a.NombreCompleto)
                .ToList();

            return View(alumnos);
        }

        // ==============================
        // NOTAS - EDITAR
        // ==============================

        // GET: /Docente/EditarNotas/1
        public IActionResult EditarNotas(int id)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos
                .Include(a => a.Notas)
                    .ThenInclude(n => n.Materia)
                .FirstOrDefault(a => a.Id == id);

            if (alumno == null)
            {
                return NotFound();
            }

            var modelo = new EditarNotasViewModel
            {
                AlumnoId = alumno.Id,
                Carne = alumno.Carne,
                NombreCompleto = alumno.NombreCompleto,
                Especialidad = alumno.Especialidad,
                GradoSeccion = alumno.GradoSeccion,
                Notas = alumno.Notas.Select(n => new NotaEditarViewModel
                {
                    NotaId = n.Id,
                    Materia = n.Materia.Nombre,
                    Parcial1 = n.Parcial1,
                    Parcial2 = n.Parcial2,
                    NotaFinal = n.NotaFinal
                }).ToList()
            };

            return View(modelo);
        }

        // POST: /Docente/EditarNotas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarNotas(EditarNotasViewModel modelo)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            foreach (var notaVm in modelo.Notas)
            {
                var nota = _context.Notas.FirstOrDefault(n => n.Id == notaVm.NotaId);
                if (nota != null)
                {
                    nota.Parcial1 = notaVm.Parcial1;
                    nota.Parcial2 = notaVm.Parcial2;
                    nota.NotaFinal = notaVm.NotaFinal;
                }
            }

            _context.SaveChanges();

            TempData["Mensaje"] = "Notas actualizadas correctamente.";

            // Volver a cargar los datos desde la BD para verlos actualizados
            return RedirectToAction(nameof(EditarNotas), new { id = modelo.AlumnoId });
        }

        // ==============================
        // ALUMNOS - CREAR
        // ==============================

        // GET: /Docente/CrearAlumno
        public IActionResult CrearAlumno()
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /Docente/CrearAlumno
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearAlumno(Alumno alumno)
        {
            if (!EsDocente())
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
        // ==============================
        // ALUMNOS - EDITAR DATOS
        // ==============================

        // GET: /Docente/EditarAlumno/5
        public IActionResult EditarAlumno(int id)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos.Find(id);
            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // POST: /Docente/EditarAlumno/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarAlumno(int id, Alumno alumno)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            if (id != alumno.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(alumno);

            _context.Update(alumno);
            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno actualizado correctamente.";
            return RedirectToAction(nameof(Alumnos));
        }


        // ==============================
        // MATERIAS - LISTAR
        // ==============================

        // GET: /Docente/Materias
        public IActionResult Materias()
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var materias = _context.Materias
                .AsNoTracking()
                .OrderBy(m => m.Nombre)
                .ToList();

            return View(materias);
        }

        // ==============================
        // MATERIAS - CREAR
        // ==============================

        // GET: /Docente/CrearMateria
        public IActionResult CrearMateria()
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: /Docente/CrearMateria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearMateria(Materia materia)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(materia);
            }

            _context.Materias.Add(materia);
            _context.SaveChanges();

            // Crear notas en 0 para todos los alumnos existentes en esta nueva materia
            var alumnos = _context.Alumnos.ToList();
            foreach (var alumno in alumnos)
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

            TempData["Mensaje"] = "Materia creada correctamente.";
            return RedirectToAction(nameof(Materias));
        }
        // ==============================
        // ALUMNOS - ELIMINAR
        // ==============================

        // GET: /Docente/EliminarAlumno/5
        public IActionResult EliminarAlumno(int id)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos
                .AsNoTracking()
                .FirstOrDefault(a => a.Id == id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // POST: /Docente/EliminarAlumno/5
        [HttpPost, ActionName("EliminarAlumno")]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarAlumnoConfirmado(int id)
        {
            if (!EsDocente())
                return RedirectToAction("Login", "Account");

            var alumno = _context.Alumnos
                .Include(a => a.Notas)
                .FirstOrDefault(a => a.Id == id);

            if (alumno == null)
                return NotFound();

            // Borrar primero las notas relacionadas
            _context.Notas.RemoveRange(alumno.Notas);

            // Luego borrar el alumno
            _context.Alumnos.Remove(alumno);
            _context.SaveChanges();

            TempData["Mensaje"] = "Alumno eliminado correctamente.";
            return RedirectToAction(nameof(Alumnos));
        }

    }
}
