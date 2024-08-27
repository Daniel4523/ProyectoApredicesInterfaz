using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Proyecto.Logica;
using System.Net.Http;
using System.Threading.Tasks;
using WebFront.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Proyecto.Controllers
{
    public class HomeController : Controller
    {
        private readonly Funciones _funciones;
        private readonly HttpClient _httpClient;

        public HomeController(Funciones funciones, HttpClient httpClient)
        {
            _funciones = funciones;
            _httpClient = httpClient;
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
                HttpContext.Session.SetString("RolUsuario", rolUsuario);
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
            string rolUsuario = HttpContext.Session.GetString("RolUsuario");
            ViewBag.RolUsuario = rolUsuario;
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
                TempData["CodeSent"] = true;
                TempData["Email"] = email;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error: " + ex.Message;
            }

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmarCodigo(string email, string recoveryCode)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(recoveryCode))
            {
                TempData["ErrorMessage"] = "Por favor, ingrese un c�digo de recuperaci�n v�lido.";
                return RedirectToAction("OlvideMiContrase�a");
            }

            try
            {
                var isCodeValid = _funciones.ValidarCodigoRecuperacion(email, recoveryCode);
                if (isCodeValid)
                {
                    return RedirectToAction("ReestablecerContrase�a", new { email = email });
                }
                else
                {
                    TempData["ErrorMessage"] = "C�digo de recuperaci�n inv�lido.";
                    return RedirectToAction("OlvideMiContrase�a");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error: " + ex.Message;
                return RedirectToAction("OlvideMiContrase�a");
            }
        }

        public IActionResult ReestablecerContrase�a(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        [HttpPost]
        public IActionResult ReestablecerContrase�a(string email, string nuevaContrase�a, string confirmarContrase�a)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nuevaContrase�a) || string.IsNullOrWhiteSpace(confirmarContrase�a))
            {
                TempData["ErrorMessage"] = "Todos los campos son obligatorios.";
                ViewData["Email"] = email;
                return View();
            }

            if (nuevaContrase�a != confirmarContrase�a)
            {
                TempData["ErrorMessage"] = "Las contrase�as no coinciden.";
                ViewData["Email"] = email;
                return View();
            }

            try
            {
                bool result = _funciones.CambiarContrase�a(email, nuevaContrase�a);
                if (result)
                {
                    TempData["SuccessMessage"] = "Contrase�a cambiada exitosamente.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo cambiar la contrase�a. Verifica la informaci�n e intenta nuevamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error: " + ex.Message;
            }

            ViewData["Email"] = email;
            return View();
        }

        public IActionResult AdminView()
        {
            var usuarios = _funciones.ObtenerUsuarios();
            ViewBag.Usuarios = usuarios;
            return View();
        }

        public IActionResult VerificarView()
        {
            return View();
        }
        public IActionResult LimpiarView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ObtenerDatos(string cuentaServicioId, string tipoServicio, string estadoServicio)
        {
            var apiUrl = $"http://localhost:5117/api/Servicios?cuentaServicioId={cuentaServicioId}&tipoServicio={tipoServicio}&estadoServicio={estadoServicio}";

            List<CuentaServicio> resultado;
            try
            {
                var response = await _httpClient.GetStringAsync(apiUrl);
                resultado = JsonConvert.DeserializeObject<List<CuentaServicio>>(response);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al obtener datos de la API: " + ex.Message;
                resultado = new List<CuentaServicio>();
            }

            ViewBag.Resultado = resultado;
            return View("VerificarView");
        }

        [HttpPost]
        public IActionResult AgregarUsuario(string email, string password, string rol)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(rol))
            {
                TempData["ErrorMessage"] = "Todos los campos son obligatorios.";
                return RedirectToAction("AdminView");
            }

            try
            {
                bool A�adirUsuario = _funciones.AgregarUsuario(email, password, rol);
                if (A�adirUsuario)
                {
                    TempData["SuccessMessage"] = "Usuario agregado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo agregar el usuario. Por favor, int�ntelo nuevamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error al agregar el usuario: " + ex.Message;
            }

            return RedirectToAction("AdminView");
        }

        [HttpPost]
        public IActionResult EliminarUsuario(string email)
        {
            try
            {
                bool eliminarUsuario = _funciones.EliminarUsuario(email);
                if (eliminarUsuario)
                {
                    TempData["SuccessMessage"] = "Usuario eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el usuario. El usuario es un administrador o no existe.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurri� un error al eliminar el usuario: " + ex.Message;
            }

            return RedirectToAction("AdminView");
        }
    }
}