using Csla.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal {
  public class DalFactory {
    private static readonly string _dalManager = "DalManager";
    private static Type _dalType;

    public static IDalManager GetManager() {
      string dalTypeName = ConfigurationManager.AppSettings[_dalManager];

      if (_dalType == null || _dalType.FullName != dalTypeName.Split(',')[0]) {
        if (!string.IsNullOrEmpty(dalTypeName))
          _dalType = Type.GetType(dalTypeName);
        else
          throw new NullReferenceException(_dalManager);
        if (_dalType == null)
          throw new ArgumentException(string.Format("Type {0} could not be found", dalTypeName));
      }

      return (IDalManager)Activator.CreateInstance(_dalType);
    }
  }
}
