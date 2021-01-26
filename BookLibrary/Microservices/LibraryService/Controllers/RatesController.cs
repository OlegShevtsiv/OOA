using System.Collections.Generic;
using System.Linq;
using Library.DataAccess.DTO;
using Library.DataProviders.Interfaces;
using Library.DataWriters.Interfaces;
using LibraryService.TransferModels;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IRateDataWriter _dataWriter;
        private readonly IRateProvider _provider;
        private readonly IBookDataWriter _bookDataWriter;
        private readonly IBookProvider _bookProvider;

        public RatesController(IRateDataWriter dataWriter, IRateProvider provider, IBookProvider bookProvider, IBookDataWriter bookDataWriter)
        {
            _dataWriter = dataWriter;
            _provider = provider;
            _bookProvider = bookProvider;
            _bookDataWriter = bookDataWriter;
        }

        [HttpPost]
        public IActionResult RateBook([FromBody]RatePostModel rate)
        {
            BookDTO bookToRate = _bookProvider.Get(rate.RatedEssenceId);
            if (bookToRate == null)
            {
                return BadRequest();
            }
            RateDTO yourRate = new RateDTO {
                                               BookId = rate.RatedEssenceId,
                                               UserId = rate.UserId,
                                               Value = rate.Value
                                           };

            List<RateDTO> allRates = _provider.GetAll().ToList();
            if (allRates.Any())
            {
                bool isFinded = false;
                foreach (var r in allRates)
                {
                    if (r.BookId == rate.RatedEssenceId && r.UserId == rate.UserId)
                    {
                        isFinded = true;
                        yourRate.Id = r.Id;
                        bookToRate.Rate += (yourRate.Value - r.Value) / bookToRate.RatesAmount;
                        _dataWriter.Update(yourRate);
                        _bookDataWriter.Update(bookToRate);
                        break;
                    }
                }
                if (!isFinded)
                {
                    uint amount = bookToRate.RatesAmount;
                    bookToRate.RatesAmount++;
                    bookToRate.Rate = (bookToRate.Rate * amount + yourRate.Value) / bookToRate.RatesAmount;
                    _bookDataWriter.Update(bookToRate);
                    _dataWriter.Add(yourRate);
                }
            }
            else
            {
                uint amount = bookToRate.RatesAmount;
                bookToRate.RatesAmount++;
                bookToRate.Rate = (bookToRate.Rate * amount + yourRate.Value) / bookToRate.RatesAmount;
                _bookDataWriter.Update(bookToRate);
                _dataWriter.Add(yourRate);
            }

            return Ok();
        }
    }
}