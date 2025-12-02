using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        public string Carne { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public string GradoSeccion { get; set; } = string.Empty;

        public List<Nota> Notas { get; set; } = new();
    }

    public class Materia
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public int UnidadesValorativas { get; set; }
    }

    public class Nota
    {
        public int Id { get; set; }

        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; } = null!;

        public int MateriaId { get; set; }
        public Materia Materia { get; set; } = null!;

        public double Parcial1 { get; set; }
        public double Parcial2 { get; set; }
        public double NotaFinal { get; set; }
    }
}

