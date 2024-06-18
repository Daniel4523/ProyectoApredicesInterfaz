using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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
                ("12", "1"),
                ("2", "2"),
                ("3","3"),
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


  

        private string codigoGenerado;

        public bool ValidarEmails(string email1, string email2)
        {
            return email1 == email2;
        }

        public string GenerarCodigo()
        {
            Random random = new Random();
            codigoGenerado = random.Next(100000, 999999).ToString();
            return codigoGenerado;
        }

        public void EnviarCorreo(string destinatario, string codigo)
        {
            string remitente = "pruebasproyectoaprendices@outlook.com"; 
            string contraseña = "123proyecto123"; 

            try
            {
                MailMessage mail = new MailMessage(remitente, destinatario);
                mail.Subject = "Código de recuperación de contraseña";
                mail.Body = $"Tu código de recuperación es: {codigo}";
                SmtpClient client = new SmtpClient("smtp.office365.com");
                client.Port = 587;
                client.Credentials = new NetworkCredential(remitente, contraseña);
                client.EnableSsl = true;

                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al enviar el correo: {ex.Message}");
            }
        }

        public bool VerificarCodigo(string codigoIngresado)
        {
            return codigoIngresado == codigoGenerado;
        }
    }
}