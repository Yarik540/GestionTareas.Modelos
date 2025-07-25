using System.Text;
using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class UsuariosController : Controller
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
            var usuarios = Crud<Usuario>.GetAll();
            return View(usuarios);
        }

        public ActionResult Details(int id)
        {
            ConfigurarToken();
            var usuario = Crud<Usuario>.GetById(id);
            var tareas = Crud<Tarea>.GetBy("usuario", id);
            ViewBag.Tareas = tareas;
            return View(usuario);
        }

        public ActionResult Create()
        {
            ConfigurarToken();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                ConfigurarToken();
                usuario.FechaCreacion = DateTime.Now;
                Crud<Usuario>.Create(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        public ActionResult Edit(int id)
        {
            ConfigurarToken();
            var usuario = Crud<Usuario>.GetById(id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                ConfigurarToken();
                Crud<Usuario>.Update(id, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        public ActionResult Delete(int id)
        {
            ConfigurarToken();
            var usuario = Crud<Usuario>.GetById(id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                ConfigurarToken();
                Crud<Usuario>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ConfigurarToken();
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }
    }
}