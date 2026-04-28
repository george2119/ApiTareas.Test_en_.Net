using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTareas.Data;
using ApiTareas.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiTareas.Controllers;

public interface ITareasController
{
    Task<IActionResult> DeleteTarea(int id);
    Task<ActionResult<Tarea>> GetTarea(int id);
    Task<ActionResult<IEnumerable<Tarea>>> GetTareas();
    Task<ActionResult<Tarea>> PostTarea(Tarea tarea);
    Task<IActionResult> PutTarea(int id, Tarea tarea);
}

[Route("api/[controller]")] // La ruta será: api/tareas
[ApiController]

[Authorize]

public class TareasController : ControllerBase, ITareasController
{
    private readonly ApplicationDbContext _context;

    public TareasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/tareas (Listar todas)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
    {

        return await _context.Tareas.ToListAsync();
    }

    // GET: api/tareas/5 (Obtener una por ID)
    [HttpGet("{id}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);

        if (tarea == null)
        {
            return NotFound(new { mensaje = $"La tarea con ID {id} no fue encontrada." });
        }

        return tarea;
    }

    // POST: api/tareas (Crear nueva)
    [HttpPost]
    public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
    {
        // 1. Validación de campos obligatorios
        if (string.IsNullOrWhiteSpace(tarea.Titulo))
        {
            return BadRequest(new { mensaje = "El título es obligatorio y no puede estar vacío." });
        }

        // 2. Validación de ID duplicado (Para evitar el Error 500 que mencionaste)
        var existeId = await _context.Tareas.AnyAsync(t => t.Id == tarea.Id);
        if (existeId)
        {
            return Conflict(new { mensaje = $"Error: Ya existe una tarea registrada con el ID {tarea.Id}. Por favor, utiliza un ID diferente o deja que el sistema lo asigne." });
        }

        try
        {
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = "Ocurrió un error inesperado al intentar guardar la tarea.", detalle = ex.Message });
        }
    }

    // PUT: api/tareas/5 (Actualizar)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarea(int id, Tarea tarea)
    {
        if (id != tarea.Id)
        {
            return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del cuerpo de la solicitud." });
        }

        // Verificar si la tarea existe antes de intentar editarla
        var tareaExistente = await _context.Tareas.AnyAsync(t => t.Id == id);
        if (!tareaExistente)
        {
            return NotFound(new { mensaje = $"No se puede actualizar: La tarea con ID {id} no existe." });
        }

        _context.Entry(tarea).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = $"Tarea {id} actualizada correctamente." });
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
    }

    // DELETE: api/tareas/5 (Eliminar)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        
        if (tarea == null)
        {
            return NotFound(new { mensaje = $"Error al eliminar: No se encontró ninguna tarea con el ID {id}." });
        }

        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = $"La tarea con ID {id} ha sido eliminada con éxito." });
    }

}