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
  public class VehicleList : ReadOnlyListBase<VehicleList, VehicleInfo> {
    #region Factory Methods

    public static VehicleList GetVehicleList() {
      return DataPortal.Fetch<VehicleList>();
    }

    public static async Task<VehicleList> GetFilteredListAsync(IEnumerable<VehicleInfo> vehicles) {
      return await DataPortal.FetchAsync<VehicleList>(vehicles);
    }

    #endregion

    #region Data Access

    [CreateChild]
    private void Child_Create() {
      // Do initialization here when creating the object.
    }

    [Fetch]
    private void Fetch() {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        IVehicleDal dal = dalManager.GetProvider<IVehicleDal>();
        IList<VehicleDto> data = null;

        data = dal.Fetch();

        if (data != null) {
          foreach (var item in data) {
            Add(DataPortal.FetchChild<VehicleInfo>(item));
          }
        }

        RaiseListChangedEvents = rlce;
        IsReadOnly = true;
      }
    }

    [Fetch]
    [RunLocal]
    private void Fetch(IEnumerable<VehicleInfo> list) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      foreach (var item in list) {
        Add(DataPortal.FetchChild<VehicleInfo>(item));
      }

      RaiseListChangedEvents = rlce;
      IsReadOnly = true;
    }

    #endregion
  }
}

