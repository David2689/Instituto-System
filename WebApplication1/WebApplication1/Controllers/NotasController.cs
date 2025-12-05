using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class NotasController : Controller
    {
        private readonly AppDbContext _context;

        public NotasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Notas  o /Notas?idAlumno=4
        public IActionResult Index(int? idAlumno)
        {
            // 1) Leemos rol y alumno en sesión
            var rol = HttpContext.Session.GetString("Rol");
            bool esAlumno = rol == "Alumno";
            int? alumnoIdSesion = HttpContext.Session.GetInt32("AlumnoId");

            // 2) Si es ALUMNO, SIEMPRE forzamos a que vea solo sus notas
            if (esAlumno)
            {
                if (alumnoIdSesion == null)
                {
                    // No debería pasar, pero por si acaso:
                    return RedirectToAction("Login", "Account");
                }

                idAlumno = alumnoIdSesion;   // Ignoramos lo que venga en la URL
            }

            // 3) Cargamos todos los alumnos (para el combo del docente)
            var alumnos = _context.Alumnos
                .OrderBy(a => a.NombreCompleto)
                .ToList();

            ViewBag.Alumnos = alumnos;
            ViewBag.EsAlumno = esAlumno;

            // 4) Si no vino idAlumno y NO es alumno, escogemos el primero
            if (idAlumno == null)
            {
                if (esAlumno && alumnoIdSesion != null)
                {
                    idAlumno = alumnoIdSesion;
                }
                else if (alumnos.Any())
                {
                    idAlumno = alumnos.First().Id;
                }
                else
                {
                    // No hay alumnos en la BD
                    return View(new List<Nota>());
                }
            }

            // 5) Buscamos el alumno seleccionado
            var alumno = _context.Alumnos.FirstOrDefault(a => a.Id == idAlumno);
            if (alumno == null)
            {
                return NotFound();
            }

            ViewBag.Alumno = alumno;

            // 6) Cargamos las notas de ese alumno
            var notas = _context.Notas
                .Include(n => n.Materia)
                .Where(n => n.AlumnoId == idAlumno)
                .OrderBy(n => n.Materia.Nombre)
                .ToList();

            return View(notas); // El modelo sigue siendo List<Nota>
        }
    }
}
