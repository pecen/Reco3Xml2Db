using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal
{
    public interface IDalManager
    {
        T GetProvider<T>() where T : class;
    }
}
