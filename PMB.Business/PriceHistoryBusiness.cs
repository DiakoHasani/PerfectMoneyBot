using PMB.Repository.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Business
{
    public class PriceHistoryBusiness: IPriceHistoryBusiness
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        public PriceHistoryBusiness(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }
    }
    public interface IPriceHistoryBusiness
    {
    }
}
