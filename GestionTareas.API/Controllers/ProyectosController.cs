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
    public class ProyectosController : ControllerBase
    {
        public readonly DbConnection connection;

        public ProyectosController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
        }

        // GET: api/proyectos
        [HttpGet]
        public IEnumerable<Proyecto> Get()
        {
            var proyectos = connection.Query<Proyecto>("SELECT * FROM proyectos");
            return proyectos;
        }

        // GET api/proyectos/5
        [HttpGet("{id}")]
        public dynamic Get(int id)
        {
            var proyecto = connection.QuerySingle<Proyecto>("SELECT * FROM proyectos WHERE id = @id", new { id });
            return proyecto;
        }

        // POST api/proyectos
        [HttpPost]
        public Proyecto Post([FromBody] Proyecto proyecto)
        {
            connection.Execute("INSERT INTO proyectos (nombre, descripcion, fechaCreacion, usuarioCreadorId) VALUES (@nombre, @descripcion, @fechaCreacion, @usuarioCreadorId)", proyecto);
            return proyecto;
        }

        // PUT api/proyectos/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Proyecto proyecto)
        {
            proyecto.Id = id;
            connection.Execute("UPDATE proyectos SET nombre = @nombre, descripcion = @descripcion, usuarioCreadorId = @usuarioCreadorId WHERE id = @id", proyecto);
        }

        // DELETE api/proyectos/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM proyectos WHERE id = @id", new { id });
        }
    }

}
