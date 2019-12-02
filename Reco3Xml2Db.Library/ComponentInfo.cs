using Csla;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentInfo : ReadOnlyBase<ComponentInfo> {
    #region Properties

    public static readonly PropertyInfo<int> ComponentIdProperty = RegisterProperty<int>(c => c.ComponentId);
    public int ComponentId {
      get { return GetProperty(ComponentIdProperty); }
      set { LoadProperty(ComponentIdProperty, value); }
    }

    public static readonly PropertyInfo<string> PDNumberProperty = RegisterProperty<string>(c => c.PDNumber);
    public string PDNumber {
      get { return GetProperty(PDNumberProperty); }
      set { LoadProperty(PDNumberProperty, value); }
    }

    public static readonly PropertyInfo<DateTime> DownloadedTimestampProperty = RegisterProperty<DateTime>(c => c.DownloadedTimestamp);
    public DateTime DownloadedTimestamp {
      get { return GetProperty(DownloadedTimestampProperty); }
      set { LoadProperty(DownloadedTimestampProperty, value); }
    }

    public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(c => c.Description);
    public string Description {
      get { return GetProperty(DescriptionProperty); }
      set { LoadProperty(DescriptionProperty, value); }
    }

    public static readonly PropertyInfo<int> PDStatusProperty = RegisterProperty<int>(c => c.PDStatus);
    public int PDStatus {
      get { return GetProperty(PDStatusProperty); }
      set { LoadProperty(PDStatusProperty, value); }
    }

    public static readonly PropertyInfo<int> ComponentTypeProperty = RegisterProperty<int>(c => c.ComponentType);
    public int ComponentType {
      get { return GetProperty(ComponentTypeProperty); }
      set { LoadProperty(ComponentTypeProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { LoadProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int> PDSourceProperty = RegisterProperty<int>(c => c.PDSource);
    public int PDSource {
      get { return GetProperty(PDSourceProperty); }
      set { LoadProperty(PDSourceProperty, value); }
    }

    public static readonly PropertyInfo<int?> SourceComponentIdProperty = RegisterProperty<int?>(c => c.SourceComponentId);
    public int? SourceComponentId {
      get { return GetProperty(SourceComponentIdProperty); }
      set { LoadProperty(SourceComponentIdProperty, value); }
    }

    #endregion

    #region Data Access

    private void Child_Fetch(ComponentDto item) {
      ComponentId = item.ComponentId;
      PDNumber = item.PDNumber;
      DownloadedTimestamp = item.DownloadedTimestamp;
      Description = item.Description;
      PDStatus = item.PDStatus;
      ComponentType = item.ComponentType;
      Xml = item.Xml;
      PDSource = item.PDSource;
      SourceComponentId = item.SourceComponentId;
    }

    #endregion
  }
}
