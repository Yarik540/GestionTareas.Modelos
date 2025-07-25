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
    public class TareasController : ControllerBase
    {
        public readonly DbConnection connection;

        public TareasController(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            connection.Open();
        }

        // GET: api/tareas
        [HttpGet]
        public IEnumerable<Tarea> Get()
        {
            var tareas = connection.Query<Tarea>("SELECT * FROM tareas");
            return tareas;
        }

        // GET api/tareas/5
        [HttpGet("{id}")]
        public dynamic Get(int id)
        {
            var tarea = connection.QuerySingle<Tarea>("SELECT * FROM tareas WHERE id = @id", new { id });
            return tarea;
        }

        // GET api/tareas/proyecto/5 - Tareas por proyecto
        [HttpGet("proyecto/{proyectoId}")]
        public IEnumerable<Tarea> GetByProyecto(int proyectoId)
        {
            var tareas = connection.Query<Tarea>("SELECT * FROM tareas WHERE proyectoId = @proyectoId", new { proyectoId });
            return tareas;
        }

        // GET api/tareas/usuario/5 - Tareas asignadas a un usuario
        [HttpGet("usuario/{usuarioId}")]
        public IEnumerable<Tarea> GetByUsuario(int usuarioId)
        {
            var tareas = connection.Query<Tarea>("SELECT * FROM tareas WHERE usuarioAsignadoId = @usuarioId", new { usuarioId });
            return tareas;
        }

        // POST api/tareas
        [HttpPost]
        public Tarea Post([FromBody] Tarea tarea)
        {
            connection.Execute("INSERT INTO tareas (titulo, descripcion, estado, prioridad, fechaCreacion, fechaVencimiento, proyectoId, usuarioAsignadoId) VALUES (@titulo, @descripcion, @estado, @prioridad, @fechaCreacion, @fechaVencimiento, @proyectoId, @usuarioAsignadoId)", tarea);
            return tarea;
        }

        // PUT api/tareas/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Tarea tarea)
        {
            tarea.Id = id;
            connection.Execute("UPDATE tareas SET titulo = @titulo, descripcion = @descripcion, estado = @estado, prioridad = @prioridad, fechaVencimiento = @fechaVencimiento, proyectoId = @proyectoId, usuarioAsignadoId = @usuarioAsignadoId WHERE id = @id", tarea);
        }

        // DELETE api/tareas/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            connection.Execute("DELETE FROM tareas WHERE id = @id", new { id });
        }
    }

}
