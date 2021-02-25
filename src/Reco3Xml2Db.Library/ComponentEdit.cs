using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;
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

    public async static Task<ComponentEdit> NewComponentEditAsync() {
      return await DataPortal.CreateAsync<ComponentEdit>();
    }

    public static ComponentEdit GetComponent(int componentId) {
      return DataPortal.Fetch<ComponentEdit>(componentId);
    }

    public static async Task<ComponentEdit> GetComponentAsync(int componentId) {
      return await DataPortal.FetchAsync<ComponentEdit>(componentId);
    }

    /// <summary>
    /// Fetch first occurence of existing component from the database
    /// </summary>
    /// <param name="pdNumber"></param>
    /// <returns></returns>
    public static ComponentEdit GetComponent(string pdNumber) {
      return DataPortal.Fetch<ComponentEdit>(pdNumber);
    }

    public static async Task<ComponentEdit> GetComponentAsync(string pdNumber) {
      return await DataPortal.FetchAsync<ComponentEdit>(pdNumber);
    }

    public static void DeleteComponent(int componentId) {
      DataPortal.Delete<ComponentEdit>(componentId);
    }

    public static async void DeleteComponentAsync(int componentId) {
      await DataPortal.DeleteAsync<ComponentEdit>(componentId);
    }

    public static bool Exists(string pdNumber) {
      var cmd = DataPortal.Create<ComponentExistsCmd>(pdNumber);
      cmd = DataPortal.Execute(cmd);

      if (!string.IsNullOrEmpty(cmd.ErrorMessage)) {
        return false;
      }

      return cmd.PDNumberExists;
    }

    #endregion

    #region Data Access

    [RunLocal]
    [Create]
    private void Create() {
      base.DataPortal_Create();
    }

    [Fetch]
    private void Fetch(int criteria) {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
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

    [Fetch]
    private void Fetch(string criteria) {
      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
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

    [Insert]
    private void Insert() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
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

    [Update]
    private void Update() {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = ctx.GetProvider<IComponentDal>();
        using (BypassPropertyChecks) {
          var item = new ComponentDto {
            ComponentId = ComponentId,
            PDNumber = PDNumber,
            DownloadedTimestamp = DownloadedTimestamp,
            Description = Description,
            PDStatus = PDStatus,
            ComponentType = ComponentType,
            Xml = Xml,
            PDSource = PDSource,
            SourceComponentId = SourceComponentId
          };
          dal.Update(item);
        }
      }
    }

    [DeleteSelf]
    private void DeleteSelf() {
      using (BypassPropertyChecks)
        Delete(ComponentId);
    }

    [Delete]
    private void Delete(int id) {
      using (var ctx = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        var dal = ctx.GetProvider<IComponentDal>();
        dal.Delete(id);
      }
    }

    #endregion
  }
}
