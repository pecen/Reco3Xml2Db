using Csla;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class RoadmapGroupInfo : ReadOnlyBase<RoadmapGroupInfo> {
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

    public static readonly PropertyInfo<int> RoadmapGroupIdProperty = RegisterProperty<int>(c => c.RoadmapGroupId);
    public int RoadmapGroupId {
      get { return GetProperty(RoadmapGroupIdProperty); }
      set { LoadProperty(RoadmapGroupIdProperty, value); }
    }

    public static readonly PropertyInfo<string> OwnerSssProperty = RegisterProperty<string>(c => c.OwnerSss);
    public string OwnerSss {
      get { return GetProperty(OwnerSssProperty); }
      set { LoadProperty(OwnerSssProperty, value); }
    }

    public static readonly PropertyInfo<string> RoadmapNameProperty = RegisterProperty<string>(c => c.RoadmapName);
    public string RoadmapName {
      get { return GetProperty(RoadmapNameProperty); }
      set { LoadProperty(RoadmapNameProperty, value); }
    }

    public static readonly PropertyInfo<bool> ProtectedProperty = RegisterProperty<bool>(c => c.Protected);
    public bool Protected {
      get { return GetProperty(ProtectedProperty); }
      set { LoadProperty(ProtectedProperty, value); }
    }

    public static readonly PropertyInfo<DateTime> CreationTimeProperty = RegisterProperty<DateTime>(c => c.CreationTime);
    public DateTime CreationTime {
      get { return GetProperty(CreationTimeProperty); }
      set { LoadProperty(CreationTimeProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { LoadProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int> ValidationStatusProperty = RegisterProperty<int>(c => c.ValidationStatus);
    public int ValidationStatus {
      get { return GetProperty(ValidationStatusProperty); }
      set { LoadProperty(ValidationStatusProperty, value); }
    }

    public static readonly PropertyInfo<int> ConvertToVehicleInputStatusProperty = RegisterProperty<int>(c => c.ConvertToVehicleInputStatus);
    public int ConvertToVehicleInputStatus {
      get { return GetProperty(ConvertToVehicleInputStatusProperty); }
      set { LoadProperty(ConvertToVehicleInputStatusProperty, value); }
    }

    #endregion

    #region Data Access

    [FetchChild]
    private void Child_Fetch(RoadmapGroupDto item) {
      RoadmapGroupId = item.RoadmapGroupId;
      OwnerSss = item.OwnerSss;
      RoadmapName = item.RoadmapName;
      Protected = item.Protected;
      CreationTime = item.CreationTime;
      //Xml = item.Xml;
      ValidationStatus = item.ValidationStatus;
      ConvertToVehicleInputStatus = item.ConvertToVehicleInputStatus;
    }

    [FetchChild]
    private void Child_Fetch(RoadmapGroupInfo item) {
      RoadmapGroupId = item.RoadmapGroupId;
      OwnerSss = item.OwnerSss;
      RoadmapName = item.RoadmapName;
      Protected = item.Protected;
      CreationTime = item.CreationTime;
      //Xml = item.Xml;
      ValidationStatus = item.ValidationStatus;
      ConvertToVehicleInputStatus = item.ConvertToVehicleInputStatus;
    }

    #endregion
  }
}
