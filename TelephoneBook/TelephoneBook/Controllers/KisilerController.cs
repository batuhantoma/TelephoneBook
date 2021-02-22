using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelephoneBook.Models;
using TelephoneBook.Services;
using TelephoneBook.Kafka;

namespace TelephoneBook.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KisilerController : ControllerBase
    {
        private readonly KisilerApiService _kisilerApiService;

        public KisilerController(KisilerApiService kisilerApiService)
        {
            _kisilerApiService = kisilerApiService;
        }

        [HttpGet]
        public ActionResult<List<Kisiler>> Get() => _kisilerApiService.Get();

        [HttpGet("{id:length(24)}", Name = "GetKisiler")]
        public ActionResult<Kisiler> Get(string id)
        {
            
            var kisiler = _kisilerApiService.Get(id);

            if (kisiler == null)
            {
                return NotFound();
            }

            return kisiler;
        }

        [HttpPost]
        public ActionResult<Kisiler> Create(Kisiler kisiler)
        {
            _kisilerApiService.Create(kisiler);

            return CreatedAtRoute("GetBook", new { id = kisiler.Id.ToString() }, kisiler);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Kisiler pIn)
        {
            var kisiler = _kisilerApiService.Get(id);

            if (kisiler == null)
            {
                return NotFound();
            }

            _kisilerApiService.Update(id, pIn);

            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var sil = _kisilerApiService.Get(id);

            if (sil == null)
            {
                return NotFound();
            }

            _kisilerApiService.Remove(sil.Id);

            return NoContent();
        }
    }
}
