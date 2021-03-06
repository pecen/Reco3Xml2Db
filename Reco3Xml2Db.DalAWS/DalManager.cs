﻿using Reco3Xml2Db.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalAWS
{
  public class DalManager : IDalManager {
    private static readonly string _typeMask = typeof(DalManager).FullName.Replace("DalManager", @"{0}");

    public T GetProvider<T>() where T : class {
      var typeName = string.Format(_typeMask, typeof(T).Name.Substring(1));
      var type = Type.GetType(typeName);
      if (type != null)
        return Activator.CreateInstance(type) as T;
      else
        throw new NotImplementedException(typeName);
    }

    public void Dispose() {
      throw new NotImplementedException();
    }
  }
}
