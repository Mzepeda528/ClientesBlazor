using System.ComponentModel.DataAnnotations;

namespace ClientesBlazor.Models
{
    public class Cliente
    {
        public int Id_cliente { get; set; }

        [Required(ErrorMessage = "El CUI es obligatorio")]
        [MaxLength(20)]
        public string CUI { get; set; } = string.Empty;

        [Required(ErrorMessage = "El NIT es obligatorio")]
        [MaxLength(20)]
        public string NIT { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MaxLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Direccion { get; set; } = string.Empty;

        [MaxLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime Fecha_Nacimiento { get; set; } = DateTime.Today.AddYears(-18);
    }
}
