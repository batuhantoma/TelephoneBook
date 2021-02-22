using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelephoneBook.Models
{
    public class ReportRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UUID { get; set; }

        public string Ad { get; set; }
        public DateTime RaporTarihi { get; set; }
        public string RaporDurumu { get; set; }

        public ReportRequest()
        {
            RaporTarihi = DateTime.Now;
        }
    }
}
