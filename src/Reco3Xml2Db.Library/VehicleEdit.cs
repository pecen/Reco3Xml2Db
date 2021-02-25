using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class VehicleEdit : BusinessBase<VehicleEdit> {
    #region Properties

    public static readonly PropertyInfo<int> VehicleIdProperty = RegisterProperty<int>(c => c.VehicleId);
    public int VehicleId {
      get { return GetProperty(VehicleIdProperty); }
      set { SetProperty(VehicleIdProperty, value); }
    }

    public static readonly PropertyInfo<string> VINProperty = RegisterProperty<string>(c => c.VIN);
    public string VIN {
      get { return GetProperty(VINProperty); }
      set { SetProperty(VINProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { SetProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int?> VehicleModeProperty = RegisterProperty<int?>(c => c.VehicleMode);
    public int? VehicleMode {
      get { return GetProperty(VehicleModeProperty); }
      set { SetProperty(VehicleModeProperty, value); }
    }

    // This is the RoadmapId in the Roadmap table
    public static readonly PropertyInfo<int> GroupIdProperty = RegisterProperty<int>(c => c.GroupId);
    public int GroupId {
      get { return GetProperty(GroupIdProperty); }
      set { SetProperty(GroupIdProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static void DeleteVehicle(int vehicleId) {
      DataPortal.Delete<VehicleEdit>(vehicleId);
    }

    public static async void DeleteVehicleAsync(int vehicleId) {
      await DataPortal.DeleteAsync<VehicleEdit>(vehicleId);
    }

    public static void DeleteOnGroupId(int groupId) {
      DataPortal.Delete<VehicleEdit>(groupId.ToString());
    }

    #endregion

    #region Data Access

    [Delete]
    private void Delete(int id) {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = ctx.GetProvider<IVehicleDal>();
        dal.Delete(id);
      }
    }

    private void Delete(string groupId) {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = ctx.GetProvider<IVehicleDal>();
        dal.DeleteOnGroupId(int.Parse(groupId));
      }
    }

    #endregion
  }
}
