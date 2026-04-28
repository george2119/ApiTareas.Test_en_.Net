using System.ComponentModel.DataAnnotations;

namespace ApiTareas.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty; // Aquí guardamos la clave encriptada

    public string Rol { get; set; } = "User"; // Por defecto todos son usuarios normales
}