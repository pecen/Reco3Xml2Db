using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum FilterableRoadmapGroupColumns {
    RoadmapGroupId,
    OwnerSss,
    [Description("Validated with success")]
    RoadmapName,
    [Description("Validated with failure")]
    ValidationStatus,
    ConvertToVehicleStatus
  }
}
