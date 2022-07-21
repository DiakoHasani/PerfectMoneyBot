using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMB.Services.Business;

namespace PMB.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceHistoryBusiness _priceHistoryBusiness;

        public PriceController(IPriceHistoryBusiness priceHistoryBusiness)
        {
            _priceHistoryBusiness = priceHistoryBusiness;
        }

        [Route("GetPrice")]
        public IActionResult GetPrice()
        {
            var priceHistory = _priceHistoryBusiness.GetLastPrice();
            if (priceHistory != null)
                return Ok(priceHistory);
            return BadRequest("nofound price in database");
        }
    }
}
