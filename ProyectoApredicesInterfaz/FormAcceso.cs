using Logicas;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProyectoApredicesInterfaz
{
    public partial class FormPrincipal : Form
    {
        private Funciones logica;

        public FormPrincipal()
        {
            InitializeComponent();


            var listTextBox = new List<TextBox> { textBox1, textBox2, textBox3 };
            logica = new Funciones(listTextBox, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string rolUsuario;
            bool ingresoExitoso = logica.Ingresar(out rolUsuario);
            if (ingresoExitoso)
            {
                MessageBox.Show("Ingreso correcto con rol: " + rolUsuario);

                Form2 form2 = new Form2(rolUsuario);
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FormRecuperarContraseña formRecuperacionContraseña = new FormRecuperarContraseña();
            formRecuperacionContraseña.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormRecuperarContraseña formRecuperacionContraseña = new FormRecuperarContraseña();
            formRecuperacionContraseña.Show();
        }
    }
}