using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum FilterableVehicleColumns {
    [Description("Vehicle Id")]
    VehicleId,
    VIN,
    [Description("Vehicle Mode")]
    VehicleMode,
    [Description("Group Id")]
    GroupId
  }
}
