using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.DataAccessLayer.Interfaces;

namespace GlobalServ.BusinessLogic.Implementations
{
    public class BaseBusinessService<T> : IBaseBusinessService<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepository;
        public BaseBusinessService(IGenericRepository<T> generic)
        {
            _genericRepository = generic;
        }
        public void Add(T entity)
        {
            _genericRepository.Add(entity);
            _genericRepository.Save();
        }

        public void Delete(T entity)
        {
           _genericRepository.Delete(entity);
            _genericRepository.Save();
        }

        public IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false,  params Expression<Func<T, object>>[] include)
        {
            return _genericRepository.Get(expression, asNoTracking, include);
        }

        public IList<T> GetAll()
        {
            return _genericRepository.GetAll();
        }

        public T? GetById<IdType>(IdType id)
        {
            return _genericRepository.GetById(id);
        }


        public void Update(T entity)
        {
            _genericRepository.Update(entity);
            _genericRepository.Save();
        }

        public void UpdateProperty(string tableName, string entityId, string property, string propetyValue)
        {
            _genericRepository.UpdateProperty(tableName, entityId, property, propetyValue);
            _genericRepository.Save();
        }
    }
}
