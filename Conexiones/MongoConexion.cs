using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Conexiones
{
    public class MongoConexion
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user")]
        public string User { get; set; }

        [BsonElement("psw")]
        public string Psw { get; set; }

        [BsonElement("rol")]
        public string Rol { get; set; }


    }
}
