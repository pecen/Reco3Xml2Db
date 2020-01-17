using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class SettingsEdit : BusinessBase<SettingsEdit> {
    #region Properties

    public static readonly PropertyInfo<string> ServerProperty = RegisterProperty<string>(c => c.Server);
    public string Server {
      get { return GetProperty(ServerProperty); }
      set { SetProperty(ServerProperty, value); }
    }

    public static readonly PropertyInfo<string> DatabaseProperty = RegisterProperty<string>(c => c.Database);
    public string Database {
      get { return GetProperty(DatabaseProperty); }
      set { SetProperty(DatabaseProperty, value); }
    }

    public static readonly PropertyInfo<int> AuthenticationProperty = RegisterProperty<int>(c => c.Authentication);
    public int Authentication {
      get { return GetProperty(AuthenticationProperty); }
      set { SetProperty(AuthenticationProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlFilePathProperty = RegisterProperty<string>(c => c.XmlFilePath);
    public string XmlFilePath {
      get { return GetProperty(XmlFilePathProperty); }
      set { SetProperty(XmlFilePathProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static SettingsEdit NewAppConfig() {
      return DataPortal.Create<SettingsEdit>();
    }

    public static SettingsEdit GetConfigSettings() {
      return DataPortal.Fetch<SettingsEdit>();
    }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create() {
      base.DataPortal_Create();
    }

    private void DataPortal_Fetch() {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerAppConfig)) {
        var dal = dalManager.GetProvider<ISettingsDal>();
        var data = dal.Fetch();
        using (BypassPropertyChecks) {
          Server = data.Server;
          Database = data.Database;
          Authentication = data.Authentication;
          XmlFilePath = data.XmlFilePath;
        }
      }
    }

    //protected override void DataPortal_Insert() {
    //  using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerAppConfig)) {
    //    var dal = ctx.GetProvider<Dal.IAppConfigDal>();
    //    using (BypassPropertyChecks) {
    //      var item = new AppConfigDto {
    //        Server = Server,
    //        Database = Database,
    //        Authentication = Authentication,
    //        XmlFilePath = XmlFilePath
    //      };
    //      dal.Insert(item);
    //    }
    //  }
    //}

    protected override void DataPortal_Update() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerAppConfig)) {
        var dal = ctx.GetProvider<ISettingsDal>();
        using (BypassPropertyChecks) {
          var item = new SettingsDto {
            Server = Server,
            Database = Database,
            Authentication = Authentication,
            XmlFilePath = XmlFilePath
          };
          dal.Update(item);
        }
      }
    }

    //protected override void DataPortal_DeleteSelf() {
    //  using (BypassPropertyChecks)
    //    DataPortal_Delete(this.Id);
    //}

    //private void DataPortal_Delete(int id) {
    //  using (var ctx = ProjectTracker.Dal.DalFactory.GetManager()) {
    //    Resources.Clear();
    //    FieldManager.UpdateChildren(this);
    //    var dal = ctx.GetProvider<ProjectTracker.Dal.IProjectDal>();
    //    dal.Delete(id);
    //  }
    //}

    #endregion
  }
}
