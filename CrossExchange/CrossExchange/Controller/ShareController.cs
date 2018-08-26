﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrossExchange.Controller
{
    [Route("api/Share")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository { get; set; }

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([FromRoute]string symbol)
        {
            var shares = _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).ToList();
            return Ok(shares);
        }


        [HttpGet("{symbol}/Latest")]
        public async Task<IActionResult> GetLatestPrice([FromRoute]string symbol)
        {
            var share = await _shareRepository.Query().Where(x => x.Symbol.Equals(symbol)).OrderByDescending(x => x.TimeStamp).FirstOrDefaultAsync();
            return Ok(share?.Rate);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HourlyShareRate value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validate: Symbol must be three characters
            if (value.Symbol.Length != 3)
            {
                return BadRequest();
            }
            else
            {
                value.Symbol = value.Symbol.ToUpper();
                value.Rate = System.Math.Round(value.Rate, 2);

                await _shareRepository.InsertAsync(value);

                return Created($"Share/{value.Id}", value);
            }
        }
                
    }
    
}