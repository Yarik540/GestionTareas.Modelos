using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionTareas.MVC.Controllers
{
    public class ProyectosController : Controller
    {
        // GET: ProyectosController
        public ActionResult Index()
        {
            var proyectos = Crud<Proyecto>.GetAll();
            return View(proyectos);
        }

        // GET: ProyectosController/Details/5
        public ActionResult Details(int id)
        {
            var proyecto = Crud<Proyecto>.GetById(id);
            var tareas = Crud<Tarea>.GetBy("proyecto", id);
            ViewBag.Tareas = tareas;
            return View(proyecto);
        }

        // GET: ProyectosController/Create
        public ActionResult Create()
        {
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View();
        }

        // POST: ProyectosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto)
        {
            try
            {
                proyecto.FechaCreacion = DateTime.Now;
                Crud<Proyecto>.Create(proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }

        // GET: ProyectosController/Edit/5
        public ActionResult Edit(int id)
        {
            var proyecto = Crud<Proyecto>.GetById(id);
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View(proyecto);
        }

        // POST: ProyectosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Proyecto proyecto)
        {
            try
            {
                Crud<Proyecto>.Update(id, proyecto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }

        // GET: ProyectosController/Delete/5
        public ActionResult Delete(int id)
        {
            var proyecto = Crud<Proyecto>.GetById(id);
            return View(proyecto);
        }

        // POST: ProyectosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Proyecto proyecto)
        {
            try
            {
                Crud<Proyecto>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(proyecto);
            }
        }
    }
}
