using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Matricula
    {
        public int Id { get; set; }

        // Alumno
        [Required]
        [MaxLength(150)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [MaxLength(20)]
        public string Sexo { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? InformacionMedica { get; set; }

        // Responsable
        [Required]
        [MaxLength(150)]
        public string NombreResponsable { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Opciones de matrícula
        [Required]
        [MaxLength(20)]
        public string Turno { get; set; } = string.Empty;  // mañana / tarde

        [Required]
        [MaxLength(80)]
        public string Especialidad { get; set; } = string.Empty;

        // Archivo (por ahora guardamos el nombre del archivo)
        [MaxLength(255)]
        public string? CertificadoRuta { get; set; }
    }
}
