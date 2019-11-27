using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library
{
  [Serializable]
  public class ComponentEdit : BusinessBase<ComponentEdit>
    {
    #region Properties

    public static readonly PropertyInfo<string> DalManagerTypeProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(DalManagerTypeProperty); }
      set { SetProperty(DalManagerTypeProperty, value); }
    }

    public static readonly PropertyInfo<string> BaseUriProperty = RegisterProperty<string>(c => c.BaseUri);
    public string BaseUri {
      get { return GetProperty(BaseUriProperty); }
      set { SetProperty(BaseUriProperty, value); }
    }

    public static readonly PropertyInfo<string> ClientSecretProperty = RegisterProperty<string>(c => c.ClientSecret);
    public string ClientSecret {
      get { return GetProperty(ClientSecretProperty); }
      set { SetProperty(ClientSecretProperty, value); }
    }

    public static readonly PropertyInfo<string> DbInUseProperty = RegisterProperty<string>(c => c.DbInUse);
    public string DbInUse {
      get { return GetProperty(DbInUseProperty); }
      set { SetProperty(DbInUseProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static ComponentEdit NewComponentEdit() {
      return DataPortal.Create<ComponentEdit>();
    }

    public static ComponentEdit GetConfigSettings() {
      return DataPortal.Fetch<ComponentEdit>();
    }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create() {
      base.DataPortal_Create();
    }

    private void DataPortal_Fetch() {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerSqlServer)) {
        var dal = dalManager.GetProvider<IComponentDal>();
        var data = dal.FetchComponents();
        using (BypassPropertyChecks) {
          //DalManagerType = data.DalManagerType;
          //BaseUri = data.BaseUri;
          //ClientSecret = data.ClientSecret;
          //DbInUse = data.DbInUse;
        }
      }
    }

    protected override void DataPortal_Insert() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerSqlServer)) {
        var dal = ctx.GetProvider<Dal.IComponentDal>();
        using (BypassPropertyChecks) {
          var item = new ComponentDto {
            //BaseUri = BaseUri,
            //ClientSecret = ClientSecret,
            //DalManagerType = DalManagerType,
            //DbInUse = DbInUse
          };
          dal.Insert(item);
        }
      }
    }

    //protected override void DataPortal_Update() {
    //  using (var ctx = DalFactory.GetManager()) {
    //    var dal = ctx.GetProvider<IComponentDal>();
    //    using (BypassPropertyChecks) {
    //      var item = new ComponentDto {
    //        //BaseUri = BaseUri,
    //        //ClientSecret = ClientSecret,
    //        //DalManagerType = DalManagerType,
    //        //DbInUse = DbInUse
    //      };
    //      dal.Update(item);
    //    }
    //  }
    //}

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
