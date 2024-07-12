using System;
using System.Windows.Forms;

namespace ProyectoApredicesInterfaz
{

    public partial class Form2 : Form
    {
        private string rol;
        public Form2(string rol)
        {
            InitializeComponent();
            this.rol = rol;
            Menu();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAdministrador formadminsitrador = new FormAdministrador();
            formadminsitrador.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Menu()
        {

            switch (rol)
            {

                case "Verificar":
                    button1.Visible = false;
                    button2.Visible = true;
                    button3.Visible = false;
                    break;
                case "Limpiar":
                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Visible = true;
                    break;
                case "Admin":
                    button1.Visible = true;
                    button2.Visible = true;
                    button3.Visible = true;
                    break;
                default:

                    button1.Visible = false;
                    button2.Visible = false;
                    button3.Visible = false;
                    break;


            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
