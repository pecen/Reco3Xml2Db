using Csla;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class VehicleInfo : ReadOnlyBase<VehicleInfo> {
    #region IsChecked

    private bool _isChecked;
    public bool IsChecked {
      get => _isChecked;
      set {
        if (value == _isChecked) return;
        _isChecked = value;
        OnPropertyChanged(nameof(IsChecked));
      }
    }

    #endregion

    #region Properties

    public static readonly PropertyInfo<int> VehicleIdProperty = RegisterProperty<int>(c => c.VehicleId);
    public int VehicleId {
      get { return GetProperty(VehicleIdProperty); }
      set { LoadProperty(VehicleIdProperty, value); }
    }

    public static readonly PropertyInfo<string> VINProperty = RegisterProperty<string>(c => c.VIN);
    public string VIN {
      get { return GetProperty(VINProperty); }
      set { LoadProperty(VINProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { LoadProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int?> VehicleModeProperty = RegisterProperty<int?>(c => c.VehicleMode);
    public int? VehicleMode {
      get { return GetProperty(VehicleModeProperty); }
      set { LoadProperty(VehicleModeProperty, value); }
    }

    // This is the RoadmapId in the Roadmap table
    public static readonly PropertyInfo<int> GroupIdProperty = RegisterProperty<int>(c => c.GroupId);
    public int GroupId {
      get { return GetProperty(GroupIdProperty); }
      set { LoadProperty(GroupIdProperty, value); }
    }

    #endregion

    #region Data Access

    [FetchChild]
    private void Child_Fetch(VehicleDto item) {
      VehicleId = item.VehicleId;
      VIN = item.VIN;
      Xml = item.Xml;
      VehicleMode = item.VehicleMode;
      GroupId = item.GroupId;
    }

    #endregion
  }
}
