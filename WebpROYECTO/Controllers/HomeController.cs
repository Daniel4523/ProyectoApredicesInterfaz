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
                // Guardar el rol en la sesi�n
                HttpContext.Session.SetString("RolUsuario", rolUsuario);

                return RedirectToAction("Inicio");
            }
            else
            {
                TempData["ErrorMessage"] = "Usuario o contrase�a incorrectos.";
                return RedirectToAction("Login");
            }
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

      
        public IActionResult ReestablecerContrase�a(string email)
        {
            return View(new { Email = email });
        }

        [HttpPost]
        public IActionResult ReestablecerContrase�a(string email, string nuevaContrase�a, string confirmarContrase�a)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nuevaContrase�a) || string.IsNullOrWhiteSpace(confirmarContrase�a))
            {
                TempData["ErrorMessage"] = "Todos los campos son obligatorios.";
                return View(new { Email = email });
            }

            if (nuevaContrase�a != confirmarContrase�a)
            {
                TempData["ErrorMessage"] = "Las contrase�as no coinciden.";
                return View(new { Email = email });
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

            return View(new { Email = email });
        }
        public IActionResult AdminView()
        {
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
    }
}
    

    
    
