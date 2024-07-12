using Logicas;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProyectoApredicesInterfaz
{
    public partial class FormRecuperarContraseña : Form
    {
        private Funciones logica;
        private string correoUsuario;
        public FormRecuperarContraseña()
        {
            InitializeComponent();
            var listTextBox = new List<TextBox> { textBox1, textBox2, textBox3 };
            logica = new Funciones(listTextBox, null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

  

        private void button2_Click(object sender, EventArgs e)
        {
            if (logica.VerificarCodigo(textBox3.Text))
            {
                MessageBox.Show("Código verificado exitosamente.");

                FormConfirmacionContraseña formConfirmacion = new FormConfirmacionContraseña(correoUsuario);
                formConfirmacion.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Código incorrecto.");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            {
                if (logica.ValidarEmails(textBox1.Text, textBox2.Text))
                {
                    string codigo = logica.GenerarCodigo();
                    try
                    {
                        logica.EnviarCorreo(textBox1.Text);
                        MessageBox.Show("El código ha sido enviado a tu correo electrónico.");
                        correoUsuario = textBox1.Text;
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

        }
    }
}
