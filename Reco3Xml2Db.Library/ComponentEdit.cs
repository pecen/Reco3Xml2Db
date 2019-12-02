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

    public static readonly PropertyInfo<int> ComponentIdProperty = RegisterProperty<int>(c => c.ComponentId);
    public int ComponentId {
      get { return GetProperty(ComponentIdProperty); }
      set { SetProperty(ComponentIdProperty, value); }
    }

    public static readonly PropertyInfo<string> PDNumberProperty = RegisterProperty<string>(c => c.PDNumber);
    public string PDNumber {
      get { return GetProperty(PDNumberProperty); }
      set { SetProperty(PDNumberProperty, value); }
    }

    public static readonly PropertyInfo<DateTime> DownloadedTimestampProperty = RegisterProperty<DateTime>(c => c.DownloadedTimestamp);
    public DateTime DownloadedTimestamp {
      get { return GetProperty(DownloadedTimestampProperty); }
      set { SetProperty(DownloadedTimestampProperty, value); }
    }

    public static readonly PropertyInfo<string> DescriptionProperty = RegisterProperty<string>(c => c.Description);
    public string Description {
      get { return GetProperty(DescriptionProperty); }
      set { SetProperty(DescriptionProperty, value); }
    }

    public static readonly PropertyInfo<int> PDStatusProperty = RegisterProperty<int>(c => c.PDStatus);
    public int PDStatus {
      get { return GetProperty(PDStatusProperty); }
      set { SetProperty(PDStatusProperty, value); }
    }

    public static readonly PropertyInfo<int> ComponentTypeProperty = RegisterProperty<int>(c => c.ComponentType);
    public int ComponentType {
      get { return GetProperty(ComponentTypeProperty); }
      set { SetProperty(ComponentTypeProperty, value); }
    }

    public static readonly PropertyInfo<string> XmlProperty = RegisterProperty<string>(c => c.Xml);
    public string Xml {
      get { return GetProperty(XmlProperty); }
      set { SetProperty(XmlProperty, value); }
    }

    public static readonly PropertyInfo<int> PDSourceProperty = RegisterProperty<int>(c => c.PDSource);
    public int PDSource {
      get { return GetProperty(PDSourceProperty); }
      set { SetProperty(PDSourceProperty, value); }
    }

    public static readonly PropertyInfo<int?> SourceComponentIdProperty = RegisterProperty<int?>(c => c.SourceComponentId);
    public int? SourceComponentId {
      get { return GetProperty(SourceComponentIdProperty); }
      set { SetProperty(SourceComponentIdProperty, value); }
    }

    #endregion

    #region Factory Methods

    public static ComponentEdit NewComponentEdit() {
      return DataPortal.Create<ComponentEdit>();
    }

    /// <summary>
    /// Fetch first occurence of existing component from the database
    /// </summary>
    /// <param name="pdNumber"></param>
    /// <returns></returns>
    public static ComponentEdit GetComponent(string pdNumber) {
      return DataPortal.Fetch<ComponentEdit>(pdNumber);
    }

    public static bool Exists(string pdNumber) {
      var cmd = DataPortal.Create<PDNumberExistsCmd>();
      cmd.PDNumber = pdNumber;
      cmd = DataPortal.Execute<PDNumberExistsCmd>(cmd);

      if (!string.IsNullOrEmpty(cmd.ErrorMessage)) {
        //ErrorMessage = cmd.ErrorMessage;
        return false;
      }

      return cmd.PDNumberExists;
    }

    #endregion

    #region Data Access

    [RunLocal]
    protected override void DataPortal_Create() {
      base.DataPortal_Create();
    }

    private void DataPortal_Fetch(string criteria) {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerSqlServer)) {
        var dal = dalManager.GetProvider<IComponentDal>();
        var data = dal.Fetch(criteria);
        if (data != null) {
          using (BypassPropertyChecks) {
            ComponentId = data.ComponentId;
            PDNumber = data.PDNumber;
            DownloadedTimestamp = data.DownloadedTimestamp;
            Description = data.Description;
            PDStatus = data.PDStatus;
            ComponentType = data.ComponentType;
            Xml = data.Xml;
            PDSource = data.PDSource;
            SourceComponentId = data.SourceComponentId;
          }
        }
      }
    }

    protected override void DataPortal_Insert() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerSqlServer)) {
        var dal = ctx.GetProvider<Dal.IComponentDal>();
        using (BypassPropertyChecks) {
          var item = new ComponentDto {
            PDNumber = PDNumber,
            DownloadedTimestamp = DownloadedTimestamp,
            Description = Description,
            PDStatus = PDStatus,
            ComponentType = ComponentType,
            Xml = Xml,
            PDSource = PDSource,
            SourceComponentId = SourceComponentId
          };
          dal.Insert(item);

          ComponentId = item.ComponentId;
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
