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
        public IActionResult Login(string usuarioIngresado, string contrase�aIngresada)
        {
            string rolUsuario;
            var resultado = _funciones.Ingresar(usuarioIngresado, contrase�aIngresada, out rolUsuario);

            if (resultado)
            {
                
                return RedirectToAction("Inicio");
            }
            else
            {

                TempData["ErrorMessage"] = "Usuario o contrase�a incorrectos.";
                return RedirectToAction("Login");
            }
        }

        public IActionResult Inicio()
        {
            return View();
        }
        public IActionResult OlvideMiContrase�a()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult OlvideMiContrase�a(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["ErrorMessage"] = "Por favor, ingrese una direcci�n de correo electr�nico v�lida.";
                return View();
            }

            try
            {
                _funciones.EnviarCorreoRecuperacion(email);
                TempData["SuccessMessage"] = "Se ha enviado un correo electr�nico con el c�digo de recuperaci�n.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error: " + ex.Message;
            }

            return View();
        }
        public IActionResult ReestablecerContrase�a(string email)
        {
         
            return View();
        }
    }
}
    
