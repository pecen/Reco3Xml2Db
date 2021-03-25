using Csla;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Library {
  [Serializable]
  public class ComponentList : ReadOnlyListBase<ComponentList, ComponentInfo> {
    protected override ComponentInfo AddNewCore() {
      var item = ComponentInfo.NewComponent(); 
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      Add(item);

      RaiseListChangedEvents = rlce;
      IsReadOnly = true;

      return item;
    }

    public void AddItem(ComponentEdit component) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      Add(component);

      RaiseListChangedEvents = rlce;
      IsReadOnly = true;
    }

    public void RemoveItem(ComponentInfo component) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      Remove(component);

      RaiseListChangedEvents = rlce;
      IsReadOnly = true;
    }

    #region Factory Methods

    public static ComponentList GetComponentList() {
      return DataPortal.Fetch<ComponentList>();
    }

    public static ComponentList GetComponentList(string pdNumber) {
      return DataPortal.Fetch<ComponentList>(pdNumber);
    }

		public static async Task<ComponentList> GetFilteredListAsync(IEnumerable<ComponentInfo> components) {
			return await DataPortal.FetchAsync<ComponentList>(components);
		}

		public static async Task<ComponentList> GetFilteredListAsync(ObservableCollection<ComponentInfo> components) {
			return await DataPortal.FetchAsync<ComponentList>(components);
		}

		#endregion

		#region Data Access

		[CreateChild]
    private void Child_Create() {
      // Do initialization here when creating the object.
    }

    [Fetch]
    private void Fetch() {
      Fetch(string.Empty);
    }

    [Fetch]
    private void Fetch(string pdNumber) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        IComponentDal dal = dalManager.GetProvider<IComponentDal>();
        IList<ComponentDto> data = null;

        if (string.IsNullOrEmpty(pdNumber)) {
          data = dal.Fetch();
        }
        else {
          data = dal.FetchAllWSamePDNumber(pdNumber);
        }

        if (data != null) {
          foreach (var item in data) {
            Add(DataPortal.FetchChild<ComponentInfo>(item));
          }
        }

        RaiseListChangedEvents = rlce;
        IsReadOnly = true;
      }
    }

    [Fetch]
    [RunLocal]
    private void Fetch(IEnumerable<ComponentInfo> list) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      foreach(var item in list) {
        Add(DataPortal.FetchChild<ComponentInfo>(item));
      }
      
      RaiseListChangedEvents = rlce;
      IsReadOnly = true;
    }

    #endregion
  }
}
