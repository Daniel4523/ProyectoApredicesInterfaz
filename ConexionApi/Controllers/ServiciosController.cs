using Microsoft.AspNetCore.Mvc;

namespace ConexionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        
           private static List<CuentaServicio> cuentasServicio = new List<CuentaServicio>
            {
                new CuentaServicio
                {
                    CuentaServicioId = "cs-123456",
                    NombreCliente = "Paul Logan",
                    Direccion = "Calle 100 #72, Ciudad",
                    FechaAlta = DateTime.Now.AddYears(-1),
                    EstadoCuenta = "activo",
                    Servicios = new List<Servicio>
                    {
                        new Servicio
                        {
                            Tipo = "voz",
                            Estado = "activo",
                            Atributos = new List<Atributo>
                            {
                                new Atributo { Nombre = "identificadorLlamadas", Valor = "activo" },
                                new Atributo { Nombre = "numeroTelefono", Valor = "300105424" },
                                new Atributo { Nombre = "planVoz", Valor = "ilimitado" }
                            }
                        },
                        new Servicio
                        {
                            Tipo = "internet",
                            Estado = "activo",
                            Atributos = new List<Atributo>
                            {
                                new Atributo { Nombre = "VelocidadSubida", Valor = "100M" },
                                new Atributo { Nombre = "VelocidadSubida", Valor = "100m" },
                                new Atributo { Nombre = "ip", Valor = "10.01.110.17" },
                                new Atributo { Nombre = "VelocidadDescarga", Valor = "200M" },
                                new Atributo { Nombre = "planInternet", Valor = "premium" }
                            }
                        }
                    },
                    HistorialServicios = new List<HistorialServicio>
                    {
                        new HistorialServicio { FechaEvento = DateTime.Now.AddMonths(-6), DescripcionEvento = "Instalación del servicio de voz" },
                        new HistorialServicio { FechaEvento = DateTime.Now.AddMonths(-3), DescripcionEvento = "Actualización del plan de internet a premium" }
                    },
                    Facturas = new List<Factura>
                    {
                        new Factura { FechaFactura = DateTime.Now.AddMonths(-1), MontoTotal = 50.00M, EstadoPago = "pagado" }
                    },
                    TicketsSoporte = new List<TicketSoporte>
                    {
                        new TicketSoporte { IdTicket = "tkt-123", FechaApertura = DateTime.Now.AddMonths(-2), DescripcionProblema = "Interrupción del servicio de internet", EstadoTicket = "cerrado" }
                    }
                },
                new CuentaServicio
                {
                    CuentaServicioId = "cs-789012",
                    NombreCliente = "Maria Gomez",
                    Direccion = "Avenida 500 #30, Ciudad",
                    FechaAlta = DateTime.Now.AddYears(-2),
                    EstadoCuenta = "suspendido",
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
                    },
                    HistorialServicios = new List<HistorialServicio>
                    {
                        new HistorialServicio { FechaEvento = DateTime.Now.AddMonths(-12), DescripcionEvento = "Contratación del servicio de televisión" },
                        new HistorialServicio { FechaEvento = DateTime.Now.AddMonths(-4), DescripcionEvento = "Suspensión temporal del servicio" }
                    },
                    Facturas = new List<Factura>
                    {
                        new Factura { FechaFactura = DateTime.Now.AddMonths(-2), MontoTotal = 40.00M, EstadoPago = "pendiente" }
                    },
                    TicketsSoporte = new List<TicketSoporte>
                    {
                        new TicketSoporte { IdTicket = "tkt-456", FechaApertura = DateTime.Now.AddMonths(-5), DescripcionProblema = "Problemas con la recepción del canal", EstadoTicket = "abierto" }
                    }
                }
            };
        public class CuentaServicioInputModel
        {
            public string CuentaServicioId { get; set; }
            public string NombreCliente { get; set; }
            public string Direccion { get; set; }
            public string EstadoCuenta { get; set; }
        }
        [HttpGet]
        public ActionResult<List<CuentaServicio>> Get([FromQuery] string cuentaServicioId = null)
        {
            var resultado = cuentasServicio;

            if (!string.IsNullOrEmpty(cuentaServicioId))
            {
                resultado = resultado
                    .Where(c => c.CuentaServicioId == cuentaServicioId)
                    .ToList();
            }

            return Ok(resultado);
        }
        [HttpPost]
        public ActionResult<CuentaServicio> Post([FromBody] CuentaServicio nuevaCuenta)
        {
            if (nuevaCuenta == null || string.IsNullOrEmpty(nuevaCuenta.CuentaServicioId))
            {
                return BadRequest("La cuenta de servicio debe tener un ID.");
            }

            cuentasServicio.Add(nuevaCuenta);
            return CreatedAtAction(nameof(Get), new { cuentaServicioId = nuevaCuenta.CuentaServicioId }, nuevaCuenta);
        }

        [HttpPut("{cuentaServicioId}")]
        public ActionResult<CuentaServicio> Put(string cuentaServicioId, [FromBody] CuentaServicioInputModel cuentaActualizada)
        {
            var cuentaExistente = cuentasServicio.FirstOrDefault(c => c.CuentaServicioId == cuentaServicioId);

            if (cuentaExistente == null)
            {
                return NotFound("Cuenta de servicio no encontrada.");
            }

            if (cuentaActualizada == null ||
                cuentaActualizada.CuentaServicioId != cuentaServicioId ||
                string.IsNullOrEmpty(cuentaActualizada.NombreCliente) ||
                string.IsNullOrEmpty(cuentaActualizada.Direccion) ||
                string.IsNullOrEmpty(cuentaActualizada.EstadoCuenta))
            {
                return BadRequest("Todos los campos (cuentaServicioId, nombreCliente, direccion, estadoCuenta) son obligatorios y el ID debe coincidir.");
            }

            cuentaExistente.NombreCliente = cuentaActualizada.NombreCliente;
            cuentaExistente.Direccion = cuentaActualizada.Direccion;
            cuentaExistente.EstadoCuenta = cuentaActualizada.EstadoCuenta;

            return Ok(cuentaExistente);
        }

     

       

        [HttpDelete("{cuentaServicioId}/{tipoDato}")]
        public ActionResult Delete(string cuentaServicioId, string tipoDato)
        {
            var cuentaExistente = cuentasServicio.FirstOrDefault(c => c.CuentaServicioId == cuentaServicioId);

            if (cuentaExistente == null)
            {
                return NotFound("Cuenta de servicio no encontrada.");
            }

            switch (tipoDato.ToLower())
            {
                case "historialservicios":
                    cuentaExistente.HistorialServicios.Clear();
                    break;
                case "facturas":
                    cuentaExistente.Facturas.Clear();
                    break;
                case "ticketssoporte":
                    cuentaExistente.TicketsSoporte.Clear();
                    break;
                case "servicios":
                    cuentaExistente.Servicios.Clear();
                    break;
                default:
                    return BadRequest("Tipo de dato no reconocido.");
            }

            return NoContent();
        }
    }
}

        
  