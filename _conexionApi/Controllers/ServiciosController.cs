using Microsoft.AspNetCore.Mvc;

namespace _conexionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        private static List<CuentaServicio> cuentasServicio = new List<CuentaServicio>
        {
            new CuentaServicio
            {
                CuentaServicioId = "CS-123456",
                Servicios = new List<Servicio>
                {
                    new Servicio
                    {
                        Tipo = "voz",
                        Estado = "activo",
                        Atributos = new List<Atributo>
                        {
                            new Atributo { Nombre = "identificadorLlamadas", Valor = "activo" }
                        }
                    },
                    new Servicio
                    {
                        Tipo = "internet",
                        Estado = "activo",
                        Atributos = new List<Atributo>
                        {
                            new Atributo { Nombre = "velocidadSubidaGPON", Valor = "100M" },
                            new Atributo { Nombre = "velocidadSubidaAAA", Valor = "100M" },
                            new Atributo { Nombre = "ip", Valor = "1.1.1.1" }
                        }
                    }
                }
            },
            new CuentaServicio
            {
                CuentaServicioId = "CS_789012",
                Servicios = new List<Servicio>
                {
                    new Servicio
                    {
                        Tipo = "televisión",
                        Estado = "activo",
                        Atributos = new List<Atributo>
                        {
                            new Atributo { Nombre = "canales", Valor = "150" },
                            new Atributo { Nombre = "paquete", Valor = "estándar" }
                        }
                    }
                }
            }
        };

        [HttpGet]
        public ActionResult<List<CuentaServicio>> Get([FromQuery] string CuentaServicio = null)
        {
            var resultado = cuentasServicio;

            if (!string.IsNullOrEmpty(CuentaServicio))
            {
                resultado = resultado
                    .Where(c => c.CuentaServicioId == CuentaServicio)
                    .ToList();
            }

            return Ok(resultado);
        }
    }
}
