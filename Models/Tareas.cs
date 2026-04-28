using System.ComponentModel.DataAnnotations;

namespace ApiTareas.Models
{
    public class Tarea
    {
        public int Id { get; set; }
        //añadiendo validaciones de seguridad
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(100, ErrorMessage = "El título no puede pasar de 100 caracteres")]

        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Completada { get; set; } = false;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}