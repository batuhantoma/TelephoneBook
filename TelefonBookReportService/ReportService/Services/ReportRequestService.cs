using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneBook.Models;

namespace TelephoneBook.Services
{
    public class ReportRequestService
    {

        private readonly IMongoCollection<ReportRequest> _report;

        public ReportRequestService(IReportStatusDatabaseSettings settings)

        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _report = database.GetCollection<ReportRequest>(settings.ApiCollectionName);
        }

        public List<ReportRequest> Get() => _report.Find(p => true).ToList();

        public ReportRequest Get(string id) => _report.Find(p => p.UUID == id).FirstOrDefault();

        public ReportRequest Create(ReportRequest report)
        {
            _report.InsertOne(report);
            return report;
        }

        public void Update(string id, ReportRequest pIn) => _report.ReplaceOne(p => p.UUID == id, pIn);
        public void Remove(ReportRequest pIn) => _report.DeleteOne(p => p.UUID == pIn.UUID);
        public void Remove(string id) => _report.DeleteOne(p => p.UUID == id);

    }
}
