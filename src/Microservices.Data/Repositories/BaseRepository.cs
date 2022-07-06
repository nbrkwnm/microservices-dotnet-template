using Microservices.Domain.Interfaces;
using Microservices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        protected readonly AppDbContext _appDbContext;

        public BaseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<TEntity> GetAll()
        {
            var query = _appDbContext.Set<TEntity>();

            if (query.Any())
                return query.ToList();

            return new List<TEntity>();
        }

        public TEntity GetById(int id)
        {
            var query = _appDbContext.Set<TEntity>().Where(c => c.Id == id);

            if (query.Any())
                return query.FirstOrDefault();

            return null;
        }

        public void Save(TEntity entity)
        {
            _appDbContext.Set<TEntity>().Add(entity);
        }
    }
}
