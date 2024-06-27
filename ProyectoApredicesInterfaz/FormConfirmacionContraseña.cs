using Logicas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoApredicesInterfaz
{
    public partial class FormConfirmacionContraseña : Form
    {
        private Funciones logica;
        private string correoUsuario; 

        public FormConfirmacionContraseña(string correo)
        {
            InitializeComponent();
            correoUsuario = correo; 
            var listTextBox = new List<TextBox> { textBox1, textBox2 };
            logica = new Funciones(listTextBox, null);
        }

            private void button1_Click(object sender, EventArgs e)
        {
            string nuevaContraseña = textBox1.Text;
            string confirmarContraseña = textBox2.Text;

            if (nuevaContraseña != confirmarContraseña)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

      
            try
            {
                bool actualizado = logica.ActualizarContraseña(correoUsuario, nuevaContraseña);

                if (actualizado)
                {
                    MessageBox.Show("Contraseña actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                    this.Close(); 
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar contraseña: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
    }
    }
}
