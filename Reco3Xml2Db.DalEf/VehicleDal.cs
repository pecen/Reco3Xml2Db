using Csla.Data.EF6;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalEf {
  public class VehicleDal : IVehicleDal {
    private readonly string _dbName = "Server";

    public List<VehicleDto> Fetch() {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var result = from r in ctx.DbContext.Vehicles
                     select new VehicleDto {
                       VehicleId = r.VehicleId,
                       VIN = r.VIN,
                       Xml = r.XML,
                       VehicleMode = r.Vehicle_Mode,
                       GroupId = r.GroupId
                     };
        return result.ToList();
      }
    }

    public VehicleDto Fetch(int vehicleId) {
      throw new NotImplementedException();
    }

    public VehicleDto Fetch(string vin) {
      throw new NotImplementedException();
    }

    public void Delete(int vehicleId) {
      using (var ctx = DbContextManager<Reco3Xml2DbContext>.GetManager(_dbName)) {
        var data = (from r in ctx.DbContext.Vehicles
                    where r.VehicleId == vehicleId
                    select r).FirstOrDefault();
        if (data != null) {
          ctx.DbContext.Vehicles.Remove(data);
          ctx.DbContext.SaveChanges();
        }
      }
    }
  }
}
