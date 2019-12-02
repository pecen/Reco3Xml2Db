using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentList : ReadOnlyListBase<ComponentList, ComponentInfo> {
    #region Factory Methods

    public static ComponentList GetComponentList(string pdNumber) {
      return DataPortal.Fetch<ComponentList>(pdNumber);
    }

    #endregion

    #region Data Access

    private void Child_Create() {
      // Do initialization here when creating the object.
    }

    private void DataPortal_Fetch(string pdNumber) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerSqlServer)) {
        IComponentDal dal = dalManager.GetProvider<IComponentDal>();
        IList<ComponentDto> data = dal.FetchAllWSamePDNumber(pdNumber);

        if (data != null) {
          foreach (var item in data) {
            Add(DataPortal.FetchChild<ComponentInfo>(item));
          }
        }
      }
    }

    #endregion
  }
}
