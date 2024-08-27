using Conexiones;
using MongoDB.Driver;
using System.Net.Mail;
using System.Net;
using System;
using System.Collections.Generic;


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
                var filtro = Builders<MongoConexion>.Filter.Eq("user", usuarioIngresado) & Builders<MongoConexion>.Filter.Eq("psw", contraseñaIngresada);
                var usuario = _basedatos.Find(filtro).FirstOrDefault();

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
                var recoveryCode = GenerarCodigo(); 
                GuardarCodigoRecuperacion(email, recoveryCode); 

                var smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("pruebasproyectoaprendices@outlook.com", "123proyecto123"),
                    EnableSsl = true,
                };

                var Mensaje = new MailMessage
                {
                    From = new MailAddress("pruebasproyectoaprendices@outlook.com"),
                    Subject = "Recuperación de Contraseña",
                    Body = $"Tu código de recuperación es: {recoveryCode}",
                    IsBodyHtml = false,
                };
                Mensaje.To.Add(email);

                smtpClient.Send(Mensaje);
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

        public bool ValidarCodigoRecuperacion(string email, string codigo)
        {
            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("email", email) & Builders<MongoConexion>.Filter.Eq("recoveryCode", codigo);
                var usuario = _basedatos.Find(filter).FirstOrDefault();

                return usuario != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al validar el código de recuperación: " + ex.Message);
            }
        }
        public bool CambiarContraseña(string email, string nuevaContraseña)
        {
            try
            {
                var filter = Builders<MongoConexion>.Filter.Eq("user", email);

            
                var update = Builders<MongoConexion>.Update.Set("psw", nuevaContraseña);

             
                var result = _basedatos.UpdateOne(filter, update);

                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al cambiar la contraseña: " + ex.Message);
            }
        }
        public bool AgregarUsuario(string email, string password, string rol)
        {
            try
            {
                var usuario = new MongoConexion
                {
                    User = email,
                    Psw = password,
                    Email = email,
                    IsConfirmed = false,
                    Rol = rol 
                };

                _basedatos.InsertOne(usuario);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el usuario: " + ex.Message);
            }
        }

        public List<MongoConexion> ObtenerUsuarios()
        {
            try
            {
                return _basedatos.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de usuarios: " + ex.Message);
            }
        }

        public bool EliminarUsuario(string email)
        {
            try
            {
                var usuario = _basedatos.Find(x => x.Email == email).FirstOrDefault();
                if (usuario == null || usuario.Rol == "Admin")
                {
                    return false;
                }

                var result = _basedatos.DeleteOne(x => x.Email == email);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario: " + ex.Message);
            }
        }
    }
}