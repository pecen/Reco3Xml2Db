using System;

namespace Reco3Xml2Db.Dal
{
    public interface IDalManager : IDisposable
    {
        T GetProvider<T>() where T : class;
    }
}
