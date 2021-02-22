using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TelephoneBook.Models
{
    public class Kisiler
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Firma { get; set; }
        public IletisimBilgisi IletisimBilgisi { get; set; }
    }

    public class IletisimBilgisi
    {
        public BilgiTipi BilgiTipi { get; set; }
        public string BilgiIcerigi { get; set; }
    }

    public class BilgiTipi
    {
        public string TelefonNumarasi { get; set; }
        public string EmailAdresi { get; set; }
        public string Konum { get; set; }
    }

}
