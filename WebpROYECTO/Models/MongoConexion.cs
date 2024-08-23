using MongoDB.Bson.Serialization.Attributes;

namespace WebFront.Models
{
    public class MongoConexion
    {
        public string User { get; set; }
        public string Psw { get; set; }
        public string Rol { get; set; }
        public string RecoveryCode { get; set; }

        public string Email { get; set; }
        public bool IsConfirmed { get; set; }
    }
}