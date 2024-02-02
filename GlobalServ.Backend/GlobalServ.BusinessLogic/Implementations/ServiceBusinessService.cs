using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.DataAccessLayer.Interfaces;
using GlobalServ.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.BusinessLogic.Implementations
{
    public class ServiceBusinessService : BaseBusinessService<ServiceModel>, IServiceBusinessService
    {
        public ServiceBusinessService(IGenericRepository<ServiceModel> generic) : base(generic)
        {
        }
    }
}
