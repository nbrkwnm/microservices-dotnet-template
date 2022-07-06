using Microservices.Domain.Interfaces;

namespace Microservices.Application
{
    public class BaseService<TEntity> where TEntity : class
    {
        private IRepository<TEntity> _repository;

        public BaseService(IRepository<TEntity> repository)
        
        {
            _repository = repository;
        }

        public TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public void Save(TEntity entity)
        {
            _repository.Save(entity);
        }
    }
}