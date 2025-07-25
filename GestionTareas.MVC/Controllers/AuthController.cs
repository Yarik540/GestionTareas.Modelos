using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using GestionTareas.API.Consumer;
using GestionTareas.Modelos.Auth;

namespace GestionTareas.MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly string _apiUrl = "https://localhost:7264/api/Auth";

        public ActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var loginData = new { Email = model.Email, Password = model.Password };
                    var json = JsonConvert.SerializeObject(loginData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync($"{_apiUrl}/login", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var loginResult = JsonConvert.DeserializeObject<dynamic>(result);
                        string token = loginResult.token.ToString();

                        HttpContext.Session.Set("Token", Encoding.UTF8.GetBytes(token));
                        HttpContext.Session.Set("Usuario", Encoding.UTF8.GetBytes(loginResult.usuario.nombre.ToString()));
                        AuthConfig.Token = token;

                        return RedirectToAction("Index", "Proyectos");
                    }
                    else
                    {
                        ViewBag.Error = "Credenciales inválidas";
                        return View(model); 
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(model); 
            }
        }
        public IActionResult Register()
        {
            return View(new Register());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Nombre) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    ViewBag.Error = "Todos los campos son obligatorios";
                    return View(model); 
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ViewBag.Error = "Las contraseñas no coinciden";
                    return View(model); 
                }

                if (model.Password.Length < 4)
                {
                    ViewBag.Error = "La contraseña debe tener al menos 4 caracteres";
                    return View(model); 
                }

                using (var client = new HttpClient())
                {
                    var usuario = new
                    {
                        Nombre = model.Nombre,
                        Email = model.Email,
                        Password = model.Password,
                        FechaCreacion = DateTime.Now
                    };

                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync("https://localhost:7264/api/Usuarios", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        ViewBag.Success = "Usuario registrado exitosamente. Ahora puedes iniciar sesión.";
                        return View(new Register()); // limpiamos el formulario de registro
                    }
                    else
                    {
                        ViewBag.Error = "Error al registrar usuario. El email ya puede estar en uso.";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            AuthConfig.Token = null;
            return RedirectToAction("Login");
        }
    }
}