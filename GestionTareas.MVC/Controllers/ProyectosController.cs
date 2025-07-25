using System.Text;
using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class ProyectosController : Controller
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
            var proyectos = Crud<Proyecto>.GetAll();
            return View(proyectos);
        }

        public ActionResult Details(int id)
        {
            ConfigurarToken();
            var proyecto = Crud<Proyecto>.GetById(id);
            var tareas = Crud<Tarea>.GetBy("proyecto", id);
            ViewBag.Tareas = tareas;
            return View(proyecto);
        }

        public ActionResult Create()
        {
            ConfigurarToken();
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto)
        {
            try
            {
                ConfigurarToken();
                proyecto.FechaCreacion = DateTime.Now;
                Crud<Proyecto>.Create(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }

        public ActionResult Edit(int id)
        {
            ConfigurarToken();
            var proyecto = Crud<Proyecto>.GetById(id);
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Proyecto proyecto)
        {
            try
            {
                ConfigurarToken();
                Crud<Proyecto>.Update(id, proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }

        public ActionResult Delete(int id)
        {
            ConfigurarToken();
            var proyecto = Crud<Proyecto>.GetById(id);
            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Proyecto proyecto)
        {
            try
            {
                ConfigurarToken();
                Crud<Proyecto>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }
    }
}