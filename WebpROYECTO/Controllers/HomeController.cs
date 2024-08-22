using Microsoft.AspNetCore.Mvc;
using Proyecto.Logica;

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        private readonly Funciones _funciones;

        public HomeController(Funciones funciones)
        {
            _funciones = funciones;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuarioIngresado, string contraseñaIngresada)
        {
            string rolUsuario;
            var resultado = _funciones.Ingresar(usuarioIngresado, contraseñaIngresada, out rolUsuario);

            if (resultado)
            {
                
                return RedirectToAction("Inicio");
            }
            else
            {

                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Inicio()
        {
            return View();
        }
        public IActionResult OlvideMiContraseña()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult OlvideMiContraseña(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Por favor, ingrese una dirección de correo electrónico válida.";
                return View();
            }

            try
            {
                _funciones.EnviarCorreoRecuperacion(email);
                TempData["SuccessMessage"] = "Se ha enviado un correo electrónico con el código de recuperación.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
            }

            return View();
        }
        public IActionResult ReestablecerContraseña(string email)
        {
         
            return View();
        }
    }
}
    
