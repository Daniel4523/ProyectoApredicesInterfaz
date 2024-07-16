using Conexiones;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Logicas
{
    public class Funciones
    {
        private List<TextBox> listTextBox;
        private IMongoCollection<MongoConexion> basedatos;
        private const string encryptionKey = "fW4ASO2gkp7TMXfHxa+r2JJU7vZAE2nks0XZSv62p7Q=";
        private const string fixedIV = "1234567890123456";





        public Funciones(List<TextBox> listTextBox, object[] objetos)
        {
            var client = new MongoClient("mongodb://localhost:27017/");
            var database = client.GetDatabase("Proyecto");
            basedatos = database.GetCollection<MongoConexion>("Usuarios");

            this.listTextBox = listTextBox;
        }

        public bool Ingresar(out string rolUsuario)
        {
            string usuarioIngresado = listTextBox[0].Text.ToLower();
            string contraseñaIngresada = listTextBox[2].Text;
            string contraseñaEncriptada = EncryptString(contraseñaIngresada);

            rolUsuario = null;

            if (!ConexionInternet())
            {
                MessageBox.Show("No tienes conexión a internet. Por favor, verifica tu conexión.", "Error de Conexión");
                return false;
            }

            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("user", usuarioIngresado) & Builders<MongoConexion>.Filter.Eq("psw", contraseñaEncriptada);
                var usuario = basedatos.Find(filter).FirstOrDefault();

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
            catch (MongoConnectionException ex)
            {
                MessageBox.Show("No se puede conectar a la base de datos. Por favor, verifica tu conexión a internet.", "Error de Conexión");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al intentar acceder a la base de datos: " + ex.Message, "Error");
                return false;
            }
        }


        private bool ConexionInternet()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send("8.8.8.8", 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }

        private string codigoGenerado;


        public bool ValidarEmails(string email1, string email2)
        {
            return string.Equals(email1, email2, StringComparison.OrdinalIgnoreCase);
        }


        public string GenerarCodigo()
        {
            Random random = new Random();
            codigoGenerado = random.Next(100000, 999999).ToString();
            return codigoGenerado;
        }

        public void EnviarCorreo(string destinatario)
        {
            string remitente = "pruebasproyectoaprendices@outlook.com";
            string contraseña = "123proyecto123";

            var filter = Builders<MongoConexion>.Filter.Regex("user", new BsonRegularExpression($"^{Regex.Escape(destinatario)}$", "i"));
            var usuario = basedatos.Find(filter).FirstOrDefault();

            if (usuario == null)
            {
                throw new Exception("Correo electrónico no encontrado en la base de datos.");
            }

            try
            {
                string codigo = GenerarCodigo();
                MailMessage mail = new MailMessage(remitente, destinatario);
                mail.Subject = "Código de recuperación de contraseña";
                mail.Body = $"Su código de recuperación es: {codigo}";
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

        public bool AgregarUsuario()
        {
            string correo = listTextBox[0].Text.ToLower();
            string confirmacionCorreo = listTextBox[1].Text.ToLower();
            string contraseña = listTextBox[2].Text;
            string confirmacionContraseña = listTextBox[3].Text;
            string rol = listTextBox[4].Text;
            string confirmacionRol = listTextBox[5].Text;

            if (!IsValidEmail(correo))
            {
                MessageBox.Show("El correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (correo != confirmacionCorreo ||
                contraseña != confirmacionContraseña ||
                rol != confirmacionRol)
            {
                MessageBox.Show("Las confirmaciones no coinciden.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!EsRolValido(rol))
            {
                MessageBox.Show("El rol especificado no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var confirmResult = MessageBox.Show(
                $"¿Desea agregar el siguiente usuario?\n\n" +
                $"Correo: {correo}\n" +
                $"Contraseña: {contraseña}\n" +
                $"Rol: {rol}\n\n",
                "Confirmar Agregar Usuario",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                string contraseñaEncriptada = EncryptString(contraseña);

                var nuevoUsuario = new MongoConexion
                {
                    User = correo,
                    Psw = contraseñaEncriptada,
                    Rol = rol
                };

                try
                {
                    basedatos.InsertOne(nuevoUsuario);
                    MessageBox.Show("Usuario agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool EsRolValido(string rol)
        {
            string[] rolesPermitidos = { "Admin", "Verificar", "Limpiar" };
            return rolesPermitidos.Contains(rol);
        }

        public bool EliminarUsuario(string correoActual)
        {
            var filter = Builders<MongoConexion>.Filter.Eq("user", correoActual);
            var usuario = basedatos.Find(filter).FirstOrDefault();

            if (usuario == null)
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            string rolActual = usuario.Rol;

            if (rolActual == "Admin")
            {
                MessageBox.Show("No puedes eliminar un usuario con rol de Admin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var confirmResult = MessageBox.Show(
                $"¿Deseas eliminar el siguiente usuario?\n\n" +
                $"Correo: {usuario.User}\n" +
                $"Rol: {rolActual}\n\n",
                "Confirmar Eliminar Usuario",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    var deleteResult = basedatos.DeleteOne(filter);
                    if (deleteResult.DeletedCount > 0)
                    {
                        MessageBox.Show("Usuario eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al eliminar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ActualizarContraseña(string usuarioActual, string nuevaContraseña)
        {
            try
            {
                string usuarioActualLower = usuarioActual.ToLower();

                var filter = Builders<MongoConexion>.Filter.Eq("user", usuarioActualLower);
                var usuario = basedatos.Find(filter).FirstOrDefault();

                if (usuario == null)
                {
                    MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                string nuevaContraseñaEncriptada = EncryptString(nuevaContraseña);
                var update = Builders<MongoConexion>.Update.Set("psw", nuevaContraseñaEncriptada);
                var updateResult = basedatos.UpdateOne(filter, update);

                if (updateResult.ModifiedCount > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la contraseña.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar contraseña: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private string EncryptString(string plainText)
        {
            byte[] key = Convert.FromBase64String(encryptionKey);
            byte[] iv = Encoding.UTF8.GetBytes(fixedIV);
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (System.IO.MemoryStream msEncrypt = new System.IO.MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }
}
