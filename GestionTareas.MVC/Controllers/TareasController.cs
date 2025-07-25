using GestionTareas.API.Consumer;
using GestionTareas.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace GestionTareas.MVC.Controllers
{
    public class TareasController : Controller
    {
        // GET: TareasController
        public ActionResult Index()
        {
            var tareas = Crud<Tarea>.GetAll();  
            return View(tareas);
        }

        // GET: TareasController/Details/5
        public ActionResult Details(int id)
        {
            var tarea = Crud<Tarea>.GetById(id);
            return View(tarea);
        }

        // GET: TareasController/Create

        public ActionResult Create(int? proyectoId)
        {
            ViewBag.Proyectos = Crud<Proyecto>.GetAll();
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            ViewBag.ProyectoSeleccionado = proyectoId;  
            return View();
        }

        // POST: TareasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            try
            {
                tarea.FechaCreacion = DateTime.Now;
                Crud<Tarea>.Create(tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Proyectos = Crud<Proyecto>.GetAll();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }

        // GET: TareasController/Edit/5
        public ActionResult Edit(int id)
        {
            var tarea = Crud<Tarea>.GetById(id);
            ViewBag.Proyectos = Crud<Proyecto>.GetAll();
            ViewBag.Usuarios = Crud<Usuario>.GetAll();
            return View(tarea);
        }

        // POST: TareasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tarea tarea)
        {
            try
            {
                Crud<Tarea>.Update(id, tarea);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Proyectos = Crud<Proyecto>.GetAll();
                ViewBag.Usuarios = Crud<Usuario>.GetAll();
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }

        // GET: TareasController/Delete/5
        public ActionResult Delete(int id)
        {
            var tarea = Crud<Tarea>.GetById(id);
            return View(tarea);
        }

        // POST: TareasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tarea tarea)
        {
            try
            {
                Crud<Tarea>.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(tarea);
            }
        }
    }
}
