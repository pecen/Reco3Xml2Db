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
  public class SettingsEdit : BusinessBase<SettingsEdit> {
    #region Properties

    public static readonly PropertyInfo<string> DalManagerTypeProperty = RegisterProperty<string>(c => c.Server);
    public string Server {
      get { return GetProperty(DalManagerTypeProperty); }
      set { SetProperty(DalManagerTypeProperty, value); }
    }

    public static readonly PropertyInfo<string> BaseUriProperty = RegisterProperty<string>(c => c.Database);
    public string Database {
      get { return GetProperty(BaseUriProperty); }
      set { SetProperty(BaseUriProperty, value); }
    }

    public static readonly PropertyInfo<int> ClientSecretProperty = RegisterProperty<int>(c => c.Authentication);
    public int Authentication {
      get { return GetProperty(ClientSecretProperty); }
      set { SetProperty(ClientSecretProperty, value); }
    }

    public static readonly PropertyInfo<string> DbInUseProperty = RegisterProperty<string>(c => c.XmlFilePath);
    public string XmlFilePath {
      get { return GetProperty(DbInUseProperty); }
      set { SetProperty(DbInUseProperty, value); }
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
        var dal = dalManager.GetProvider<IAppConfigDal>();
        var data = dal.Fetch();
        using (BypassPropertyChecks) {
          Server = data.Server;
          Database = data.Database;
          Authentication = data.Authentication;
          XmlFilePath = data.XmlFilePath;
        }
      }
    }

    protected override void DataPortal_Insert() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerAppConfig)) {
        var dal = ctx.GetProvider<Dal.IAppConfigDal>();
        using (BypassPropertyChecks) {
          var item = new AppConfigDto {
            Server = Server,
            Database = Database,
            Authentication = Authentication,
            XmlFilePath = XmlFilePath
          };
          dal.Insert(item);
        }
      }
    }

    protected override void DataPortal_Update() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerAppConfig)) {
        var dal = ctx.GetProvider<IAppConfigDal>();
        using (BypassPropertyChecks) {
          var item = new AppConfigDto {
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
