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
    public partial class FormAdministrador : Form
    {
        private Funciones logica;
        public FormAdministrador()
        {
            InitializeComponent();
            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBox1);
            listTextBox.Add(textBox2);
            listTextBox.Add(textBox3);
            listTextBox.Add(textBox4);
            listTextBox.Add(textBox5);
            listTextBox.Add(textBox6);
            listTextBox.Add(textBox7);
            Object[] objetos = { };
            logica = new Funciones(listTextBox, objetos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logica.AgregarUsuario();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string correoActual = textBox7.Text;
            

            bool eliminado = logica.EliminarUsuario(correoActual);
            if (eliminado)
            {

            }
            else
            {

            }
        }
    }
}
