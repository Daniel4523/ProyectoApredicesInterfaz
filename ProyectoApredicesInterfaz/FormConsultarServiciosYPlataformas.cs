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
                    // Aquí puedes mostrar los datos en tu formulario
                    MessageBox.Show($"Nombre del Cliente: {cuentaServicio.NombreCliente}");
                    // Asigna los datos a los controles correspondientes
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
                var response = await client.GetAsync("http://localhost:5105/api/Services");
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var cuentaServicio = JsonSerializer.Deserialize<CuentaServicio>(jsonString);

                return cuentaServicio;
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}