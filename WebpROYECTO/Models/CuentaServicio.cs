namespace WebFront.Models
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
            public List<Servicio> Servicios { get; set; }
        }
    
}
