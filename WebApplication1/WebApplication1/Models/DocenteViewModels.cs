using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class NotaEditarViewModel
    {
        public int NotaId { get; set; }

        public string Materia { get; set; } = string.Empty;

        [Display(Name = "Parcial 1")]
        public double Parcial1 { get; set; }

        [Display(Name = "Parcial 2")]
        public double Parcial2 { get; set; }

        [Display(Name = "Nota final")]
        public double NotaFinal { get; set; }
    }

    public class EditarNotasViewModel
    {
        public int AlumnoId { get; set; }

        public string Carne { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string GradoSeccion { get; set; } = string.Empty;

        public List<NotaEditarViewModel> Notas { get; set; } = new();
    }
}

