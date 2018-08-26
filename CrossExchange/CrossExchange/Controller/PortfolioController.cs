using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CrossExchange.Controller
{
    [Route("api/Portfolio")]
    public class PortfolioController : ControllerBase
    {
        private IPortfolioRepository _portfolioRepository { get; set; }

        public PortfolioController(IShareRepository shareRepository, ITradeRepository tradeRepository, IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet("{portFolioid}")]
        public async Task<IActionResult> GetPortfolioInfo([FromRoute]int portFolioid)
        {
            var portfolio = _portfolioRepository.GetAll().Where(x => x.Id.Equals(portFolioid));

            return Ok(portfolio);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Portfolio value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validate name already exists
            var portfolio = _portfolioRepository.Query().Where(x => x.Name.Equals(value.Name)).FirstOrDefault();

            if (portfolio != null && portfolio.Id > 0) return BadRequest();
            
            //Catch error if there are data errors from the parameter
            try
            {
                await _portfolioRepository.InsertAsync(value);

                return Created($"Portfolio/{value.Id}", value);
            }
            catch (System.Exception)
            {
                return BadRequest(ModelState);
            }
        }

    }
}
