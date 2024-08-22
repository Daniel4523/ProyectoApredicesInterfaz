using Conexiones;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
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
   
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string cuentaServicioId = textBox1.Text;
            if (string.IsNullOrEmpty(cuentaServicioId))
            {
                MessageBox.Show("Por favor, ingrese un ID de Cuenta Servicio.");
                return;
            }

            try
            {
                var cuentaServicio = await GetCuentaServicioAsync(cuentaServicioId);
                if (cuentaServicio != null)
                {
                    ShowCuentaServicioDetails(cuentaServicio);
                    FillDataGridViews(cuentaServicio);
                }
                else
                {
                    MessageBox.Show("No se encontraron datos para el ID de Cuenta Servicio proporcionado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener los datos: {ex.Message}");
            }
        }

        private async Task<ConexionApi.CuentaServicio> GetCuentaServicioAsync(string cuentaServicioId)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7181/api/Servicios");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var cuentaServicios = JsonSerializer.Deserialize<List<ConexionApi.CuentaServicio>>(jsonString, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                     
                        if (cuentaServicios != null)
                        {
                            return cuentaServicios.Find(cs => cs.cuentaServicioId == cuentaServicioId);
                        }
                        else
                        {
                            MessageBox.Show("La lista de cuentaServicios es null.");
                        }
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Error al deserializar el JSON: {ex.Message}");
                    }
                }
                else
                {
                    var errorString = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error en la respuesta de la API: {errorString}");
                }
            }
            return null;
        }

        private void ShowCuentaServicioDetails(ConexionApi.CuentaServicio cuentaServicio)
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

        private void FillDataGridViews(ConexionApi.CuentaServicio cuentaServicio)
        {
     
            dataGridView1.Columns.Clear();
            dataGridView2.Columns.Clear();
            dataGridView3.Columns.Clear();
            dataGridView4.Columns.Clear();
            dataGridView5.Columns.Clear();

          
            dataGridView1.Columns.Add("ID de la Cuenta", "ID de la Cuenta");
            dataGridView1.Columns.Add("Nombre del Cliente", "Nombre del Cliente");
            dataGridView1.Columns.Add("Dirección", "Dirección");
            dataGridView1.Columns.Add("Fecha de Alta", "Fecha de Alta");
            dataGridView1.Columns.Add("Estado de la Cuenta", "Estado de la Cuenta");

            dataGridView1.Rows.Add(cuentaServicio.cuentaServicioId, cuentaServicio.nombreCliente, cuentaServicio.direccion, cuentaServicio.fechaAlta, cuentaServicio.estadoCuenta);

          
            dataGridView2.Columns.Add("Tipo de Servicio", "Tipo de Servicio");
            dataGridView2.Columns.Add("Estado del Servicio", "Estado del Servicio");
            dataGridView2.Columns.Add("Atributos", "Atributos");

            foreach (var servicio in cuentaServicio.servicios)
            {
                string atributos = string.Join(", ", servicio.atributos.ConvertAll(a => $"{a.nombre}: {a.valor}"));
                dataGridView2.Rows.Add(servicio.tipo, servicio.estado, atributos);
            }

        
            dataGridView3.Columns.Add("Fecha del Evento", "Fecha del Evento");
            dataGridView3.Columns.Add("Descripción del Evento", "Descripción del Evento");

            foreach (var historial in cuentaServicio.historialServicios)
            {
                dataGridView3.Rows.Add(historial.fechaEvento, historial.descripcionEvento);
            }

        
            dataGridView4.Columns.Add("Fecha de la Factura", "Fecha de la Factura");
            dataGridView4.Columns.Add("Monto Total", "Monto Total");
            dataGridView4.Columns.Add("Estado del Pago", "Estado del Pago");

            foreach (var factura in cuentaServicio.facturas)
            {
                dataGridView4.Rows.Add(factura.fechaFactura, factura.montoTotal, factura.estadoPago);
            }

          
            dataGridView5.Columns.Add("ID del Ticket", "ID del Ticket");
            dataGridView5.Columns.Add("Fecha de Apertura", "Fecha de Apertura");
            dataGridView5.Columns.Add("Descripción del Problema", "Descripción del Problema");
            dataGridView5.Columns.Add("Estado del Ticket", "Estado del Ticket");

            foreach (var ticket in cuentaServicio.ticketsSoporte)
            {
                dataGridView5.Rows.Add(ticket.idTicket, ticket.fechaApertura, ticket.descripcionProblema, ticket.estadoTicket);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}

