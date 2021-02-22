using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneBook.Models;
using TelephoneBook.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TelephoneBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportRequestController : ControllerBase
    {
        private readonly ReportRequestService _reportRequestService;

        public ReportRequestController(ReportRequestService reportApiService)
        {
            _reportRequestService = reportApiService;
        }

        [HttpGet]
        public ActionResult<List<ReportRequest>> Get() => _reportRequestService.Get();

        [HttpPost]
        public ActionResult<ReportRequest> Create(ReportRequest report)
        {
            report.RaporDurumu = "Hazırlanıyor.";
            var result = _reportRequestService.Create(report);
            var Kafka = new TelephoneBook.Kafka.Kafka();
            Kafka.ProduceData(result);

            return result;
        }
    }
}
