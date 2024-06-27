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
    public partial class FormPrincipal : Form
    {
        private Funciones logica;

        public FormPrincipal()
        {
            InitializeComponent();

          
            var listTextBox = new List<TextBox>();
            listTextBox.Add(textBox1); 
            listTextBox.Add(textBox2); 
            Object[] objetos = { };

            logica = new Funciones(listTextBox, objetos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ingresoExitoso = logica.ingresar();
            if (ingresoExitoso)
            {
                MessageBox.Show("ingreso correcto");
                Form2 seleccion = new Form2();
                seleccion.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Error. Verifique sus creedenciales o la conexión a internet.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormRecuperacionContraseña formRecuperacionContraseña = new FormRecuperacionContraseña();
            formRecuperacionContraseña.Show();
           
        }
    }
}
