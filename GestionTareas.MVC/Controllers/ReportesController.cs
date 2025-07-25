using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GestionTareas.MVC.Controllers
{
    public class ReportesController : Controller
    {
        private void ConfigurarToken()
        {
            var tokenBytes = HttpContext.Session.Get("Token");
            if (tokenBytes != null)
            {
                var token = Encoding.UTF8.GetString(tokenBytes);
                AuthConfig.Token = token;
            }
        }

        public ActionResult Index()
        {
            ConfigurarToken();
              if (string.IsNullOrEmpty(AuthConfig.Token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var tareas = Crud<Tarea>.GetAll();
            var proyectos = Crud<Proyecto>.GetAll();
            var usuarios = Crud<Usuario>.GetAll();

            ViewBag.Proyectos = proyectos;
            ViewBag.Usuarios = usuarios;

            return View(tareas);
        }

        [HttpPost]
        public ActionResult Filtrar(string estado, string prioridad, string proyecto, string usuario, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            ConfigurarToken();
            var tareas = Crud<Tarea>.GetAll();
            var proyectos = Crud<Proyecto>.GetAll();
            var usuarios = Crud<Usuario>.GetAll();

            // Aplicamos filtros con LINQ
            var tareasFiltradas = tareas.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                tareasFiltradas = tareasFiltradas.Where(t => t.Estado == estado);

            if (!string.IsNullOrEmpty(prioridad))
                tareasFiltradas = tareasFiltradas.Where(t => t.Prioridad == prioridad);

            if (!string.IsNullOrEmpty(proyecto) && int.TryParse(proyecto, out int proyectoId))
                tareasFiltradas = tareasFiltradas.Where(t => t.ProyectoId == proyectoId);

            if (!string.IsNullOrEmpty(usuario) && int.TryParse(usuario, out int usuarioId))
                tareasFiltradas = tareasFiltradas.Where(t => t.UsuarioAsignadoId == usuarioId);

            if (fechaDesde.HasValue)
                tareasFiltradas = tareasFiltradas.Where(t => t.FechaVencimiento >= fechaDesde.Value);

            if (fechaHasta.HasValue)
                tareasFiltradas = tareasFiltradas.Where(t => t.FechaVencimiento <= fechaHasta.Value);

            ViewBag.Proyectos = proyectos;
            ViewBag.Usuarios = usuarios;
            ViewBag.FiltrosAplicados = true;

            return View("Index", tareasFiltradas.ToList());
        }
    }
}