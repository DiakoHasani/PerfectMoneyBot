using AutoMapper;
using PMB.Model.DTO.PriceHistory;
using PMB.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Business.AutoMapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<AddPriceHistoryModel, TblPriceHistory>();
            CreateMap<TblPriceHistory, AddPriceHistoryModel>();

            CreateMap<PriceHistoryModel, TblPriceHistory>();
            CreateMap<TblPriceHistory, PriceHistoryModel>();
        }
    }
}
