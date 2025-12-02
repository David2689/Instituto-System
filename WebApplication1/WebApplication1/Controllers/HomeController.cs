using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models; // Asegúrate de que estás usando tu modelo adecuado

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        // Acción principal que redirige a la vista de inicio
        public IActionResult Index()
        {
            return View(); // Retorna la vista Index.cshtml
        }

        // Acción para la vista Matricula
        [HttpGet]  // Acción para obtener la página de matrícula
        public IActionResult Matricular()
        {
            return View(); // Retorna la vista Matricula.cshtml
        }

        // Acción para manejar el envío de los datos del formulario de matrícula
        [HttpPost]
        public IActionResult Matricular(MatriculaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Aquí puedes manejar el formulario y realizar acciones como guardar en la base de datos
                return RedirectToAction("Index"); // O redirigir a otra acción después de guardar
            }

            return View(model); // Si los datos no son válidos, vuelve a mostrar el formulario con errores
        }
    }
}
