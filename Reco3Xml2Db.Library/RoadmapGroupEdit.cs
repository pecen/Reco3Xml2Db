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
  public class RoadmapGroupEdit : BusinessBase<RoadmapGroupEdit> {
    #region Properties

    public static readonly PropertyInfo<int> RoadmapGroupIdProperty = RegisterProperty<int>(c => c.RoadmapGroupId);
    public int RoadmapGroupId {
      get { return GetProperty(RoadmapGroupIdProperty); }
      set { SetProperty(RoadmapGroupIdProperty, value); }
    }

    public static readonly PropertyInfo<string> OwnerSssProperty = RegisterProperty<string>(c => c.OwnerSss);
    public string OwnerSss {
      get { return GetProperty(OwnerSssProperty); }
      set { SetProperty(OwnerSssProperty, value); }
    }

    public static readonly PropertyInfo<string> RoadmapNameProperty = RegisterProperty<string>(c => c.RoadmapName);
    public string RoadmapName {
      get { return GetProperty(RoadmapNameProperty); }
      set { SetProperty(RoadmapNameProperty, value); }
    }

    public static readonly PropertyInfo<bool> ProtectedProperty = RegisterProperty<bool>(c => c.Protected);
    public bool Protected {
      get { return GetProperty(ProtectedProperty); }
      set { SetProperty(ProtectedProperty, value); }
    }

    public static readonly PropertyInfo<DateTime> CreationTimeProperty = RegisterProperty<DateTime>(c => c.CreationTime);
    public DateTime CreationTime {
      get { return GetProperty(CreationTimeProperty); }
      set { SetProperty(CreationTimeProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { SetProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int> ValidationStatusProperty = RegisterProperty<int>(c => c.ValidationStatus);
    public int ValidationStatus {
      get { return GetProperty(ValidationStatusProperty); }
      set { SetProperty(ValidationStatusProperty, value); }
    }

    public static readonly PropertyInfo<int> ConvertToVehicleInputStatusProperty = RegisterProperty<int>(c => c.ConvertToVehicleInputStatus);
    public int ConvertToVehicleInputStatus {
      get { return GetProperty(ConvertToVehicleInputStatusProperty); }
      set { SetProperty(ConvertToVehicleInputStatusProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static async void DeleteRoadmapGroupAsync(int roadmapGroupId) {
      await DataPortal.DeleteAsync<RoadmapGroupEdit>(roadmapGroupId);
    }

    #endregion

    #region Data Access

    [Delete]
    private void Delete(int id) {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = ctx.GetProvider<IRoadmapGroupDal>();
        dal.Delete(id);
      }
    }

    #endregion
  }
}
