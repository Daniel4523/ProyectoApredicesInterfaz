using Conexiones;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Proyecto.Logica
{
    public class Funciones
    {
        private readonly IMongoCollection<MongoConexion> _basedatos;
        private string codigoGenerado;
        public Funciones(IMongoClient client)
        {
            var database = client.GetDatabase("Proyecto");
            _basedatos = database.GetCollection<MongoConexion>("Usuarios");
        }

        public bool Ingresar(string usuarioIngresado, string contraseñaIngresada, out string rolUsuario)
        {
            rolUsuario = null;

            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("user", usuarioIngresado) & Builders<MongoConexion>.Filter.Eq("psw", contraseñaIngresada);
                var usuario = _basedatos.Find(filter).FirstOrDefault();

                if (usuario != null)
                {
                    rolUsuario = usuario.Rol;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (MongoConnectionException)
            {
                throw new Exception("No se puede conectar a la base de datos. Por favor, verifica tu conexión a internet.");
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar acceder a la base de datos: " + ex.Message);
            }
        }
        public string GenerarCodigo()
        {
            Random random = new Random();
            codigoGenerado = random.Next(100000, 999999).ToString();
            return codigoGenerado;
        }

        public bool EnviarCorreoRecuperacion(string email)
        {
            try
            {

                var uniqueCode = Guid.NewGuid().ToString();

            
                GuardarCodigoRecuperacion(email, uniqueCode);

                var smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("pruebasproyectoaprendices@outlook.com", "123proyecto123"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("pruebasproyectoaprendices@outlook.com"),
                    Subject = "Recuperación de Contraseña",
                    Body = $"Haz clic en el siguiente enlace para restablecer tu contraseña: <a href='http://localhost:5286/Home/ReestablecerContraseña?email={email}&code={uniqueCode}'>Restablecer Contraseña</a>",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al enviar el correo electrónico: " + ex.Message);
            }
        }

        public bool ConfirmarCorreo(string email)
        {
            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("email", email);
                var update = Builders<MongoConexion>.Update.Set("isConfirmed", true);
                var result = _basedatos.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al confirmar el correo en la base de datos: " + ex.Message);
            }
        }

        private void GuardarCodigoRecuperacion(string email, string codigo)
        {
            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("email", email);
                var update = Builders<MongoConexion>.Update.Set("recoveryCode", codigo);
                _basedatos.UpdateOne(filter, update);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al guardar el código de recuperación: " + ex.Message);
            }
        }
    }
}
