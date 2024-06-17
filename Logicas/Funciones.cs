using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Logicas
{
    public class Funciones
    {
        private List<TextBox> listTextBox;
        private List<(string usuario, string contraseña)> listaUsuarios;

        public Funciones(List<TextBox> listTextBox, object[] objetos)
        {
            this.listTextBox = listTextBox;

            listaUsuarios = new List<(string usuario, string contraseña)>
            {
                ("1", "1"),
                ("2", "2"),
            
            };
        }

        public bool ingresar()
        {
            string usuarioIngresado = listTextBox[0].Text;
            string contraseñaIngresada = listTextBox[1].Text;

            foreach (var (usuario, contraseña) in listaUsuarios)
            {
                if (usuario == usuarioIngresado && contraseña == contraseñaIngresada)
                {
                    return true; 
                }
            }

            return false; 
        }
    }
}

