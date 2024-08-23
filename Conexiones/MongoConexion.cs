using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Conexiones
{
    public class MongoConexion
    {

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user")]
        public string User { get; set; }

        [BsonElement("psw")]
        public string Psw { get; set; }

        [BsonElement("rol")]
        public string Rol { get; set; }

        [BsonElement("recoveryCode")]
        public string RecoveryCode { get; set; }

        [BsonElement("email")] // Este campo es necesario si se usa en consultas
        public string Email { get; set; }

        [BsonElement("isConfirmed")] // Campo adicional si estás confirmando emails
        public bool IsConfirmed { get; set; }
    }
}
