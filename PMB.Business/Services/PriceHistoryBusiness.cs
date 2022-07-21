using AutoMapper;
using PMB.Model.DTO.PriceHistory;
using PMB.Model.General;
using PMB.Repository.Domain;
using PMB.Repository.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PMB.Services.Business
{
    public class PriceHistoryBusiness : IPriceHistoryBusiness
    {
        private readonly IMapper _mapper;
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        public PriceHistoryBusiness(IPriceHistoryRepository priceHistoryRepository,
            IMapper mapper)
        {
            _priceHistoryRepository = priceHistoryRepository;
            _mapper = mapper;
        }

        public async Task<MessageModel> Add(AddPriceHistoryModel model)
        {
            _priceHistoryRepository.Add(_mapper.Map<TblPriceHistory>(model));
            if (await _priceHistoryRepository.SaveChangeAsync() > 0)
            {
                return new MessageModel
                {
                    Result = true,
                    Message = "success add all prices to database"
                };
            }
            else
            {
                return new MessageModel
                {
                    Result = false,
                    Message = "error add prices to database"
                };
            }
        }

        public PriceHistoryModel GetLastPrice()
        {
            return _mapper.Map<PriceHistoryModel>(_priceHistoryRepository.GetAll().OrderByDescending(a => a.Id).FirstOrDefault());
        }
    }
    public interface IPriceHistoryBusiness
    {
        Task<MessageModel> Add(AddPriceHistoryModel model);
        PriceHistoryModel GetLastPrice();
    }
}
