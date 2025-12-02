using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Linq;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    public class NotasController : Controller
    {
        private readonly AppDbContext _context;

        public NotasController(AppDbContext context)
        {
            _context = context;
        }

        private bool EsAlumno()
        {
            return HttpContext.Session.GetString("Rol") == "Alumno";
        }

        private bool EsDocente()
        {
            return HttpContext.Session.GetString("Rol") == "Docente";
        }

        // GET: /Notas   o   /Notas?idAlumno=2
        public IActionResult Index(int? idAlumno)
        {
            // 1) Obtener todos los alumnos para el selector
            var alumnos = _context.Alumnos
                .AsNoTracking()
                .OrderBy(a => a.NombreCompleto)
                .ToList();

            if (!alumnos.Any())
            {
                return Content("No hay alumnos registrados.");
            }

            // 2) Determinar qué alumno mostrar
            int alumnoId;

            if (idAlumno.HasValue && idAlumno.Value > 0)
            {
                alumnoId = idAlumno.Value;
            }
            else
            {
                // si no se manda id, usamos el primer alumno
                alumnoId = alumnos.First().Id;
            }

            var alumno = _context.Alumnos
                .Include(a => a.Notas)
                    .ThenInclude(n => n.Materia)
                .FirstOrDefault(a => a.Id == alumnoId);

            if (alumno == null)
            {
                return NotFound();
            }

            var modelo = new AlumnoNotasViewModel
            {
                Carne = alumno.Carne,
                NombreCompleto = alumno.NombreCompleto,
                Especialidad = alumno.Especialidad,
                GradoSeccion = alumno.GradoSeccion,
                Notas = alumno.Notas.Select(n => new NotaMateriaViewModel
                {
                    Materia = n.Materia.Nombre,
                    Parcial1 = n.Parcial1,
                    Parcial2 = n.Parcial2,
                    NotaFinal = n.NotaFinal
                }).ToList()
            };

            // 3) Enviar lista de alumnos y el id seleccionado a la vista
            ViewBag.Alumnos = alumnos;
            ViewBag.AlumnoSeleccionadoId = alumnoId;

            return View(modelo);
        }
    }
}
