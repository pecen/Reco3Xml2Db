using Csla.Data.EF6;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using Reco3Xml2Db.DalEf.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Reco3Xml2Db.DalEf {
  public class ComponentDal : IComponentDal {
    private readonly string _dbName = "Server";

    public bool Exists(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = (from r in ctx.DbContext.Components
                      where r.PDNumber == pdNumber
                      select r.ComponentId).Count() > 0;
        return result;
      }
    }

    public List<ComponentDto> Fetch() {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.Components
                     select new ComponentDto {
                       ComponentId = r.ComponentId,
                       PDNumber = r.PDNumber,
                       DownloadedTimestamp = r.DownloadedTimestamp,
                       Description = r.Description,
                       PDStatus = r.PD_Status,
                       ComponentType = r.Component_Type,
                       Xml = r.XML,
                       PDSource = r.PD_Source,
                       SourceComponentId = r.SourceComponentId
                     };
        return result.ToList();
      }
    }

    public ComponentDto Fetch(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = (from r in ctx.DbContext.Components
                      where r.PDNumber == pdNumber
                      select new ComponentDto {
                        ComponentId = r.ComponentId,
                        PDNumber = r.PDNumber,
                        DownloadedTimestamp = r.DownloadedTimestamp,
                        Description = r.Description,
                        PDStatus = r.PD_Status,
                        ComponentType = r.Component_Type,
                        Xml = r.XML,
                        PDSource = r.PD_Source,
                        SourceComponentId = r.SourceComponentId
                      }).FirstOrDefault();
        return result;
      }
    }

    public List<ComponentDto> FetchAllWSamePDNumber(string pdNumber) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.Components
                     where r.PDNumber.Contains(pdNumber)
                     select new ComponentDto {
                       ComponentId = r.ComponentId,
                       PDNumber = r.PDNumber,
                       DownloadedTimestamp = r.DownloadedTimestamp,
                       Description = r.Description,
                       PDStatus = r.PD_Status,
                       ComponentType = r.Component_Type,
                       Xml = r.XML,
                       PDSource = r.PD_Source,
                       SourceComponentId = r.SourceComponentId
                     };
        return result.ToList();
      }
    }

    public void Insert(ComponentDto data) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var item = new Reco3Component {
          ComponentId = data.ComponentId,
          PDNumber = data.PDNumber,
          DownloadedTimestamp = data.DownloadedTimestamp,
          Description = data.Description,
          PD_Status = data.PDStatus,
          Component_Type = data.ComponentType,
          XML = data.Xml,
          PD_Source = data.PDSource,
          SourceComponentId = data.SourceComponentId
        };

        ctx.DbContext.Components.Add(item);
        ctx.DbContext.SaveChanges();
        data.ComponentId = item.ComponentId;
      }
    }
  }
}
