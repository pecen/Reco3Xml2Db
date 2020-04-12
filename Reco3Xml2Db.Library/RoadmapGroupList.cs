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
  public class RoadmapGroupList : ReadOnlyListBase<RoadmapGroupList, RoadmapGroupInfo> {
    #region Factory Methods

    public static RoadmapGroupList GetRoadmapGroups() {
      return DataPortal.Fetch<RoadmapGroupList>();
    }

    public static async Task<RoadmapGroupList> GetFilteredListAsync(IEnumerable<RoadmapGroupInfo> vehicles) {
      return await DataPortal.FetchAsync<RoadmapGroupList>(vehicles);
    }

    #endregion

    #region Data Access

    [Fetch]
    private void Fetch() {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      using (var dalManager = DalFactory.GetManager(DalManagerTypes.DalManagerDb)) {
        IRoadmapGroupDal dal = dalManager.GetProvider<IRoadmapGroupDal>();
        IList<RoadmapGroupDto> data = null;

        data = dal.Fetch();

        if (data != null) {
          foreach (var item in data) {
            Add(DataPortal.FetchChild<RoadmapGroupInfo>(item));
          }
        }

        RaiseListChangedEvents = rlce;
        IsReadOnly = true;
      }
    }

    [Fetch]
    [RunLocal]
    private void Fetch(IEnumerable<RoadmapGroupInfo> list) {
      var rlce = RaiseListChangedEvents;
      RaiseListChangedEvents = false;
      IsReadOnly = false;

      foreach (var item in list) {
        Add(DataPortal.FetchChild<RoadmapGroupInfo>(item));
      }

      RaiseListChangedEvents = rlce;
      IsReadOnly = true;
    }

    #endregion
  }
}
