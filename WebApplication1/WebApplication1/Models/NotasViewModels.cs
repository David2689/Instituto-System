using System.Collections.Generic;

namespace WebApplication1.Models
{
    // Representa las notas de una materia
    public class NotaMateriaViewModel
    {
        public string Materia { get; set; } = string.Empty;
        public double Parcial1 { get; set; }
        public double Parcial2 { get; set; }
        public double NotaFinal { get; set; }

        // Aprobado si la nota final es >= 6.0
        public bool Aprobado => NotaFinal >= 6.0;
    }

    // Representa el conjunto de notas de un alumno
    public class AlumnoNotasViewModel
    {
        public string Carne { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string GradoSeccion { get; set; } = string.Empty;

        public List<NotaMateriaViewModel> Notas { get; set; } = new();
    }
}
