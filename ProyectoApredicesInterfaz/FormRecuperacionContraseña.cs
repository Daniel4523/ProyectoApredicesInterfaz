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
    public partial class FormRecuperacionContraseña : Form
    {
        private Funciones logica;
       
        public FormRecuperacionContraseña()
        {

            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBox1);
            listTextBox.Add(textBox2);
            listTextBox.Add(textBox3);
            Object[] objetos = { };

            logica = new Funciones(listTextBox, objetos);
            InitializeComponent();
     
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            if (logica.ValidarEmails(textBox1.Text, textBox2.Text))
            {
                string codigo = logica.GenerarCodigo();
                try
                {
                    logica.EnviarCorreo(textBox1.Text, codigo);
                    MessageBox.Show("El código ha sido enviado a tu correo electrónico.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Los correos electrónicos no coinciden.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (logica.VerificarCodigo(textBox3.Text))
            {
                MessageBox.Show("Código verificado exitosamente.");
                FormConfirmacionContraseña formularioConfirmacion = new FormConfirmacionContraseña();
                formularioConfirmacion.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Código incorrecto.");
            }
        }
    }
}
