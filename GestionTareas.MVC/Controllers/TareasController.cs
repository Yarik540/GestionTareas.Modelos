using System.Text;
using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class TareasController : Controller
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
            return View(tareas);
        }

        public ActionResult Details(int id)
        {
            ConfigurarToken();
            var tarea = Crud<Tarea>.GetById(id);
            return View(tarea);
        }

        public ActionResult Create(int? proyectoId)
        {
            ConfigurarToken();
            ViewBag.Proyectos = Crud<Proyecto>.GetAll();
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            ViewBag.ProyectoSeleccionado = proyectoId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            try
            {
                ConfigurarToken();
                tarea.FechaCreacion = DateTime.Now;
                Crud<Tarea>.Create(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ViewBag.Proyectos = Crud<Proyecto>.GetAll();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }

        public ActionResult Edit(int id)
        {
            ConfigurarToken();
            var tarea = Crud<Tarea>.GetById(id);
            ViewBag.Proyectos = Crud<Proyecto>.GetAll();
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tarea tarea)
        {
            try
            {
                ConfigurarToken();
                Crud<Tarea>.Update(id, tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ViewBag.Proyectos = Crud<Proyecto>.GetAll();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }

        public ActionResult Delete(int id)
        {
            ConfigurarToken();
            var tarea = Crud<Tarea>.GetById(id);
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tarea tarea)
        {
            try
            {
                ConfigurarToken();
                Crud<Tarea>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }
    }
}