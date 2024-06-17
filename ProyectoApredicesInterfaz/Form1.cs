﻿using Logicas;
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
    public partial class Form1 : Form
    {
        private Funciones logica;

        public Form1()
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
                MessageBox.Show("ingreso dsnvosndsd.hv");
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("usuario o contrseña incorrectos.");
            }

        }
    }
}
