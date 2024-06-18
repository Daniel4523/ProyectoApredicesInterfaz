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
    }
}
