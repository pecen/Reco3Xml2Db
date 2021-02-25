using Csla.Data.EF6;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalEf {
  public class RoadmapGroupDal : IRoadmapGroupDal {
    private readonly string _dbName = "Server";

    public void Delete(int roadmapGroupId) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var data = (from r in ctx.DbContext.RoadmapGroups
                    where r.RoadmapGroupId == roadmapGroupId
                    select r).FirstOrDefault();
        if (data != null) {
          ctx.DbContext.RoadmapGroups.Remove(data);
          ctx.DbContext.SaveChanges();
        }
      }
    }

    public IList<RoadmapGroupDto> Fetch() {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.RoadmapGroups
                     select new RoadmapGroupDto {
                       RoadmapGroupId = r.RoadmapGroupId,
                       OwnerSss = r.OwnerSss,
                       RoadmapName = r.RoadmapName,
                       Protected = r.Protected,
                       CreationTime = r.CreationTime,
                       Xml = r.XML,
                       ValidationStatus = r.Validation_Status,
                       ConvertToVehicleInputStatus = r.ConvertToVehicleInput_Status
                     };
        return result.ToList();
      }
    }
  }
}
