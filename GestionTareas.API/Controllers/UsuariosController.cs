using System.Data.Common;
using Dapper;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        public readonly DbConnection connection;

        public UsuariosController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
        }

        // GET: api/usuarios
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            var usuarios = connection.Query<Usuario>("SELECT * FROM usuarios");
            return usuarios;
        }

        // GET api/usuarios/5
        [HttpGet("{id}")]
        public Usuario Get(int id)
        {
            var usuario = connection.QuerySingle<Usuario>("SELECT * FROM usuarios WHERE id = @id", new { id });
            return usuario;
        }

        // POST api/usuarios
        [HttpPost]
        [AllowAnonymous]
        public Usuario Post([FromBody] Usuario usuario)
        {
            connection.Execute("INSERT INTO usuarios (nombre, email, password, fechaCreacion) VALUES (@nombre, @email, @password, @fechaCreacion)", usuario);
            return usuario;
        }

        // PUT api/usuarios/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Usuario usuario)
        {
            usuario.Id = id;
            connection.Execute("UPDATE usuarios SET nombre = @nombre, email = @email, password = @password WHERE id = @id", usuario);
        }

        // DELETE api/usuarios/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM usuarios WHERE id = @id", new { id });
        }
    }


}
