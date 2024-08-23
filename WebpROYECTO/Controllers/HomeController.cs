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
                // Guardar el rol en la sesión
                HttpContext.Session.SetString("RolUsuario", rolUsuario);

                return RedirectToAction("Inicio");
            }
            else
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        public IActionResult ConfirmarCodigo(string email, string recoveryCode)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(recoveryCode))
            {
                TempData["ErrorMessage"] = "Por favor, ingrese un código de recuperación válido.";
                return RedirectToAction("OlvideMiContraseña");
            }

            try
            {
                var isCodeValid = _funciones.ValidarCodigoRecuperacion(email, recoveryCode);
                if (isCodeValid)
                {
                    return RedirectToAction("ReestablecerContraseña", new { email = email });
                }
                else
                {
                    TempData["ErrorMessage"] = "Código de recuperación inválido.";
                    return RedirectToAction("OlvideMiContraseña");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
                return RedirectToAction("OlvideMiContraseña");
            }
        }
        public IActionResult Inicio()
        {
         
            string rolUsuario = HttpContext.Session.GetString("RolUsuario");

           
            ViewBag.RolUsuario = rolUsuario;

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
                TempData["CodeSent"] = true; 
                TempData["Email"] = email;  
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
            }

            return View();
        }

      
        public IActionResult ReestablecerContraseña(string email)
        {
            return View(new { Email = email });
        }

        [HttpPost]
        public IActionResult ReestablecerContraseña(string email, string nuevaContraseña, string confirmarContraseña)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nuevaContraseña) || string.IsNullOrWhiteSpace(confirmarContraseña))
            {
                TempData["ErrorMessage"] = "Todos los campos son obligatorios.";
                return View(new { Email = email });
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden.";
                return View(new { Email = email });
            }

            try
            {
                bool result = _funciones.CambiarContraseña(email, nuevaContraseña);
                if (result)
                {
                    TempData["SuccessMessage"] = "Contraseña cambiada exitosamente.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo cambiar la contraseña. Verifica la información e intenta nuevamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error: " + ex.Message;
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
    

    
    
