using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalServ.Core.Interfaces
{
    public interface IGeneratePasswordHelper
    {
        public string GetSalt();
        public string GetHash(string text);
    }
}
