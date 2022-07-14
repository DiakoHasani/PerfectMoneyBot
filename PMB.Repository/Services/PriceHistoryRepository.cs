using PMB.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Repository.Services
{
    public class PriceHistoryRepository : BaseRepository<TblPriceHistory>, IPriceHistoryRepository
    {
        public PriceHistoryRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IPriceHistoryRepository : IBaseRepository<TblPriceHistory>
    {
    }
}
