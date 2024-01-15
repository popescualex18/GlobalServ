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
        protected readonly IGenericRepository<T> GenericRepository;
        public BaseBusinessService(IGenericRepository<T> generic)
        {
            GenericRepository = generic;
        }
        public void Add(T entity)
        {
            GenericRepository.Add(entity);
            GenericRepository.Save();
        }

        public void Delete(T entity)
        {
           GenericRepository.Delete(entity);
            GenericRepository.Save();
        }

        public IList<T> Get(Expression<Func<T, bool>> expression, bool asNoTracking = false,  params Expression<Func<T, object>>[] include)
        {
            return GenericRepository.Get(expression, asNoTracking, include);
        }

        public IList<T> GetAll()
        {
            return GenericRepository.GetAll();
        }

        public T? GetById<IdType>(IdType id)
        {
            return GenericRepository.GetById(id);
        }


        public void Update(T entity)
        {
            GenericRepository.Update(entity);
            GenericRepository.Save();
        }

        public void UpdateProperty(string tableName, string entityId, string property, string propetyValue)
        {
            GenericRepository.UpdateProperty(tableName, entityId, property, propetyValue);
            GenericRepository.Save();
        }
    }
}
