using Microsoft.AspNetCore.Mvc;


namespace ConexionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiciosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<CuentaServicio>> Get()
        {
            var cuentasServicio = new List<CuentaServicio>
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
                new CuentaServicio{


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

            return Ok(cuentasServicio);

        }

    }
}