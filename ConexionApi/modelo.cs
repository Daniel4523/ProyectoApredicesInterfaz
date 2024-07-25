namespace ConexionApi
{

        public class Servicio
        {
            public string Tipo { get; set; }
            public string Estado { get; set; }
            public List<Atributo> Atributos { get; set; }
        }

        public class Atributo
        {
            public string Nombre { get; set; }
            public string Valor { get; set; }
        }

        public class CuentaServicio
        {
            public string CuentaServicioId { get; set; }
            public string NombreCliente { get; set; }
            public string Direccion { get; set; }
            public DateTime FechaAlta { get; set; }
            public string EstadoCuenta { get; set; }
            public List<Servicio> Servicios { get; set; }
            public List<HistorialServicio> HistorialServicios { get; set; }
            public List<Factura> Facturas { get; set; }
            public List<TicketSoporte> TicketsSoporte { get; set; }
        }

        public class HistorialServicio
        {
            public DateTime FechaEvento { get; set; }
            public string DescripcionEvento { get; set; }
        }

        public class Factura
        {
            public DateTime FechaFactura { get; set; }
            public decimal MontoTotal { get; set; }
            public string EstadoPago { get; set; }
        }

        public class TicketSoporte
        {
            public string IdTicket { get; set; }
            public DateTime FechaApertura { get; set; }
            public string DescripcionProblema { get; set; }
            public string EstadoTicket { get; set; }
        }

    }
