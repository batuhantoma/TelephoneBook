using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelephoneBook.Models;
using TelephoneBook.Services;
using System.IO;
using CsvHelper;
using FileHelpers;

namespace TelephoneBook.Kafka
{
    public class Kafka
    {
        public void ProduceData()
        {
            try
            {
                var conf = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };

                Action<DeliveryReport<Null, string>> handler = r =>
                    Console.WriteLine(!r.Error.IsError
                        ? $"Delivered message to {r.TopicPartitionOffset}"
                        : $"Delivery Error: {r.Error.Reason}");

                using (var p = new ProducerBuilder<Null, string>(conf).Build())
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        p.Produce("userReport", new Message<Null, string> { Value = i.ToString() }, handler);
                    }

                    // wait for up to 10 seconds for any inflight messages to be delivered.
                    p.Flush(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public class Report{
            public string Konum { get; set; }
            public int Count { get; set; }
        }

        public void ConsumerData()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("userReport");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {

                            KisilerDatabaseSettings ksettings = new KisilerDatabaseSettings()
                            {
                                ApiCollectionName = "TelephoneBookCollection",
                                ConnectionString = "mongodb://localhost:27017",
                                DatabaseName = "Book"
                            };
                            KisilerApiService kisislerService = new KisilerApiService(ksettings);
                            var data = kisislerService.Get();
                            var cr = c.Consume(cts.Token);

                            var groupedData = data.GroupBy(x => new { x.IletisimBilgisi.BilgiTipi.Konum }).Select(y => new { y.Key.Konum, Count = y.Count() }).ToList(); ;
                           
                            //// GroupedData csv ye atılır.
                            
                            ReportStatusDatabaseSettings settings = new ReportStatusDatabaseSettings()
                            {
                                ApiCollectionName = "ReportStatus",
                                ConnectionString = "mongodb://localhost:27017",
                                DatabaseName = "Book"
                            };
                            ReportRequestService reportRequestService = new ReportRequestService(settings);
                            var report = reportRequestService.Get(cr.Value);
                            report.RaporDurumu = "Tamamlandı";
                            reportRequestService.Update(cr.Value, report);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }
        }
    }
}
