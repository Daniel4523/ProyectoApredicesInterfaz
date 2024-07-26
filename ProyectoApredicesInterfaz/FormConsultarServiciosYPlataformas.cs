using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ProyectoApredicesInterfaz
{
    public partial class FormConsultarServiciosYPlataformas : Form
    {
        public FormConsultarServiciosYPlataformas()
        {
            InitializeComponent();
        }

        private async void FormConsultarServiciosYPlataformas_Load(object sender, EventArgs e)
        {
            try
            {
                var cuentaServicio = await GetCuentaServicioAsync();
                if (cuentaServicio != null)
                {
                    ShowCuentaServicioDetails(cuentaServicio);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}");
            }
        }

        private async Task<CuentaServicio> GetCuentaServicioAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7181/api/Servicios");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var cuentaServicio = JsonSerializer.Deserialize<CuentaServicio>(jsonString);

                return cuentaServicio;
            }
        }

        private void ShowCuentaServicioDetails(CuentaServicio cuentaServicio)
        {
            MessageBox.Show($"ID de la Cuenta: {cuentaServicio.cuentaServicioId}\nNombre del Cliente: {cuentaServicio.nombreCliente}\nDirección: {cuentaServicio.direccion}\nFecha de Alta: {cuentaServicio.fechaAlta}\nEstado de la Cuenta: {cuentaServicio.estadoCuenta}");

            foreach (var servicio in cuentaServicio.servicios)
            {
                string atributos = string.Join("\n", servicio.atributos.ConvertAll(a => $"  Nombre: {a.nombre}, Valor: {a.valor}"));
                MessageBox.Show($"Tipo de Servicio: {servicio.tipo}\nEstado: {servicio.estado}\nAtributos:\n{atributos}");
            }

            foreach (var historial in cuentaServicio.historialServicios)
            {
                MessageBox.Show($"Fecha del Evento: {historial.fechaEvento}\nDescripción del Evento: {historial.descripcionEvento}");
            }

            foreach (var factura in cuentaServicio.facturas)
            {
                MessageBox.Show($"Fecha de la Factura: {factura.fechaFactura}\nMonto Total: {factura.montoTotal}\nEstado del Pago: {factura.estadoPago}");
            }

            foreach (var ticket in cuentaServicio.ticketsSoporte)
            {
                MessageBox.Show($"ID del Ticket: {ticket.idTicket}\nFecha de Apertura: {ticket.fechaApertura}\nDescripción del Problema: {ticket.descripcionProblema}\nEstado del Ticket: {ticket.estadoTicket}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}
