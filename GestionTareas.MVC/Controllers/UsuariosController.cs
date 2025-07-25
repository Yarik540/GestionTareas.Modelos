using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTareas.MVC.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: UsuariosController
        public ActionResult Index()
        {
            var usuarios = Crud<Usuario>.GetAll();
            return View(usuarios);
        }

        // GET: UsuariosController/Details/5
        public ActionResult Details(int id)
        {
            var usuario = Crud<Usuario>.GetById(id);
            var tareas = Crud<Tarea>.GetBy("usuario", id);
            ViewBag.Tareas = tareas;
            return View(usuario);
        }

        // GET: UsuariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsuariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            try
            {
                usuario.FechaCreacion = DateTime.Now;
                Crud<Usuario>.Create(usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        // GET: UsuariosController/Edit/5
        public ActionResult Edit(int id)
        {
            var usuario = Crud<Usuario>.GetById(id);
            return View(usuario);
        }

        // POST: UsuariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                Crud<Usuario>.Update(id, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }

        // GET: UsuariosController/Delete/5
        public ActionResult Delete(int id)
        {
            var usuario = Crud<Usuario>.GetById(id);
            return View(usuario);
        }

        // POST: UsuariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                Crud<Usuario>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(usuario);
            }
        }
    }
}
