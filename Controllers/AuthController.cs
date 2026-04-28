using ApiTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTareas.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiTareas.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UsuarioDto request)
    {
        // 1. Encriptar la contraseña
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var usuario = new Usuario
        {
            Username = request.Username,
            PasswordHash = passwordHash
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok("Usuario registrado con éxito");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UsuarioDto request)
    {
        // 1. Buscar el usuario en la BD
        //var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == request.Username);
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (usuario == null) return Unauthorized("Usuario no encontrado");

        // 2. Verificar si la contraseña coincide con el Hash
        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
        {
            return Unauthorized("Contraseña incorrecta");
        }

        // 3. Si todo está bien, generar el Token
        var token = GenerarJwtToken(usuario);
        return Ok(new { token });
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "ClaveSuperSecretaDeRespaldoMuyLarga"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Role, usuario.Rol),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

// DTO para recibir datos sin exponer el modelo real
public class UsuarioDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}