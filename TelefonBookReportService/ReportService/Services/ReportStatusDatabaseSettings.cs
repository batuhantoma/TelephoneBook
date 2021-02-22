using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelephoneBook.Models
{
    public class ReportStatusDatabaseSettings : IReportStatusDatabaseSettings
    {
        public string ApiCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IReportStatusDatabaseSettings
    {
        string ApiCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
