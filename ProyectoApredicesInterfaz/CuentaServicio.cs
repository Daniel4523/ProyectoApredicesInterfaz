using System;
using System.Collections.Generic;

namespace ProyectoApredicesInterfaz
{

    public class Servicio
    {
        public string tipo { get; set; }
        public string estado { get; set; }
        public List<Atributo> atributos { get; set; }
    }

    public class Atributo
    {
        public string nombre { get; set; }
        public string valor { get; set; }
    }

    public class CuentaServicio
    {
        public string cuentaServicioId { get; set; }
        public string nombreCliente { get; set; }
        public string direccion { get; set; }
        public DateTime fechaAlta { get; set; }
        public string estadoCuenta { get; set; }
        public List<Servicio> servicios { get; set; }
        public List<HistorialServicio> historialServicios { get; set; }
        public List<Factura> facturas { get; set; }
        public List<TicketSoporte> ticketsSoporte { get; set; }
    }

    public class HistorialServicio
    {
        public DateTime fechaEvento { get; set; }
        public string descripcionEvento { get; set; }
    }

    public class Factura
    {
        public DateTime fechaFactura { get; set; }
        public decimal montoTotal { get; set; }
        public string estadoPago { get; set; }
    }

    public class TicketSoporte
    {
        public string idTicket { get; set; }
        public DateTime fechaApertura { get; set; }
        public string descripcionProblema { get; set; }
        public string estadoTicket { get; set; }
    }
}
