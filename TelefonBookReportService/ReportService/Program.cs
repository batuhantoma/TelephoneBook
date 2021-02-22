using System;
using TelephoneBook.Kafka;

namespace ReportService
{
    class Program
    {
        static void Main(string[] args)
        {
            Kafka k = new Kafka();
            k.ConsumerData();
        }
    }
}
