using Microsoft.AspNetCore.Mvc;


namespace ConexionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<CuentaServicio> Get()
        {
            var cuentaServicio = new CuentaServicio
            {
                CuentaServicioId = "cs-123456",
                NombreCliente = "Juan Perez",
                Direccion = "Calle Falsa 123, Ciudad",
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
                       new Atributo { Nombre = "numeroTelefono", Valor = "555-1234" },
                       new Atributo { Nombre = "planVoz", Valor = "ilimitado" }
                   }
               },
               new Servicio
               {
                   Tipo = "internet",
                   Estado = "activo",
                   Atributos = new List<Atributo>
                   {
                       new Atributo { Nombre = "VelocidadSubidaGPON", Valor = "100M" },
                       new Atributo { Nombre = "VelocidadSubidaAAA", Valor = "100m" },
                       new Atributo { Nombre = "ip", Valor = "1.1.0.1" },
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
            };

            return Ok(cuentaServicio);
        }
    }
}

