using Logicas;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProyectoApredicesInterfaz
{
    public partial class FormAdministrador : Form
    {
        private Funciones logica;
        public FormAdministrador()
        {
            InitializeComponent();
            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBox5);
            listTextBox.Add(textBox8);
            listTextBox.Add(textBox9);
            listTextBox.Add(textBox10);
            listTextBox.Add(textBox11);
            listTextBox.Add(textBox12);
            listTextBox.Add(textBox13);
            Object[] objetos = { };
            logica = new Funciones(listTextBox, objetos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logica.AgregarUsuario();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string correoActual = textBox13.Text;


            bool eliminado = logica.EliminarUsuario(correoActual);
            if (eliminado)
            {

            }
            else
            {

            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
