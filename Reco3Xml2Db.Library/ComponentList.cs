using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;
using System.Collections.Generic;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentList : ReadOnlyListBase<ComponentList, ComponentInfo> {
    #region Factory Methods

    public static ComponentList GetComponentList() {
      return DataPortal.Fetch<ComponentList>();
    }

    public static ComponentList GetComponentList(string pdNumber) {
      return DataPortal.Fetch<ComponentList>(pdNumber);
    }

    #endregion

    #region Data Access

    [CreateChild]
    private void Child_Create() {
      // Do initialization here when creating the object.
    }

    [Fetch]
    private void Fetch() {
      Fetch(null);
    }

    [Fetch]
    private void Fetch(string pdNumber) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        IComponentDal dal = dalManager.GetProvider<IComponentDal>();
        IList<ComponentDto> data = null;

        if (string.IsNullOrEmpty(pdNumber)) {
          data = dal.Fetch();
        }
        else {
          data = dal.FetchAllWSamePDNumber(pdNumber);
        }

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
