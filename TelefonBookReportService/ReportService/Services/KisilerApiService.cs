using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneBook.Models;

namespace TelephoneBook.Services
{
    public class KisilerApiService
    {
        private readonly IMongoCollection<Kisiler> _kisiler;

        public KisilerApiService(IKisilerDatabaseSettings settings)
        
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _kisiler = database.GetCollection<Kisiler>(settings.ApiCollectionName);
        }

        public List<Kisiler> Get() => _kisiler.Find(p => true).ToList();

        public Kisiler Get(string id) => _kisiler.Find(p => p.Id == id).FirstOrDefault();

        public Kisiler Create(Kisiler kisi)
        {
            _kisiler.InsertOne(kisi);
            return kisi;
        }

        public void Update(string id, Kisiler pIn) => _kisiler.ReplaceOne(p => p.Id == id, pIn);
        public void Remove(Kisiler pIn) => _kisiler.DeleteOne(p => p.Id == pIn.Id);
        public void Remove(string id) => _kisiler.DeleteOne(p => p.Id == id);

    }
}
