using GestionTareas.Modelos.Auth;
using GestionTareas.Modelos;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Dapper;

namespace GestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DbConnection connection;

        public AuthController(IConfiguration config)
        {
            _config = config;
            var connectionString = config.GetConnectionString("DefaultConnection");
            connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            var usuario = connection.QuerySingleOrDefault<Usuario>(
                "SELECT * FROM usuarios WHERE email = @email AND password = @password",
                new { email = login.Email, password = login.Password });

            if (usuario != null)
            {
                var token = GenerateJwtToken(usuario);
                return Ok(new { Token = token, Usuario = usuario });
            }

            return Unauthorized("Credenciales inválidas");
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
