using Microsoft.EntityFrameworkCore;
using PMB.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PMB.Repository.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private DataContext _dataContext;
        DbSet<T> db;

        public BaseRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
            db = dataContext.Set<T>();
        }

        public void Add(T entity)
        {
            db.Add(entity);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> where = null)
        {
            if (where != null)
                return db.Where(where);
            return db;
        }

        public T GetById(int id)
        {
            return db.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await db.FindAsync(id);
        }

        public void Remove(T entity)
        {
            db.Remove(entity);
        }

        public int SaveChange()
        {
            return _dataContext.SaveChanges();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
    }

    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        T GetById(int id);
        IQueryable<T> GetAll(Expression<Func<T, bool>> where = null);
        void Remove(T entity);
        void Add(T entity);
        void Update(T entity);
        Task<int> SaveChangeAsync();
        int SaveChange();
    }
}
