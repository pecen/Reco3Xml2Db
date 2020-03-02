using Csla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentInfoCreator : ReadOnlyBase<ComponentInfoCreator> {
    public static readonly PropertyInfo<ComponentInfo> ResultProperty = RegisterProperty<ComponentInfo>(c => c.Result);
    public ComponentInfo Result {
      get { return GetProperty(ResultProperty); }
      set { LoadProperty(ResultProperty, value); }
    }

    public static ComponentInfoCreator GetComponentInfoCreator() {
      return DataPortal.Fetch<ComponentInfoCreator>();
    }

    [Fetch]
    private void Fetch() {
      Result = DataPortal.CreateChild<ComponentInfo>();
    }
  }
}
