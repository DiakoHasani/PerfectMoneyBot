using PMB.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMB.Repository.Services
{
    public class ErrorRepository : BaseRepository<TblError>, IErrorRepository
    {
        public ErrorRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
    public interface IErrorRepository : IBaseRepository<TblError>
    {
    }
}
