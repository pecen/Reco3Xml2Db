using Csla.Data;
using Reco3Xml2Db.Dal;
using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalSql {
  public class VehicleDal : IVehicleDal {
    private readonly string _dbName = "Server";

    public void Delete(int vehicleId) {
      throw new NotImplementedException();
    }

    public List<VehicleDto> Fetch() {
      using (var ctx = ConnectionManager<SqlConnection>.GetManager(_dbName)) {
        using (var cm = ctx.Connection.CreateCommand()) {
          cm.CommandType = CommandType.Text;
          cm.CommandText = "SELECT * FROM Vehicle"; // WHERE PDNumber = @pdNumber";
          //cm.Parameters.AddWithValue("@pdNumber", pdNumber);

          using (var dr = cm.ExecuteReader()) {
            if (dr.HasRows) {
              var result = new List<VehicleDto>();
              while (dr.Read()) {
                var vehicle = new VehicleDto {
                  VehicleId = dr.GetInt32(0),
                  VIN = dr.GetString(1),
                  Xml = dr.GetString(2),
                  VehicleMode = dr.GetInt32(3),
                  GroupId = dr.GetInt32(4),
                };

                result.Add(vehicle);
              }

              return result;
            }
            else {
              return null;
            }
          }
        }
      }
    }

    public VehicleDto Fetch(int vehicleId) {
      throw new NotImplementedException();
    }

    public VehicleDto Fetch(string vin) {
      throw new NotImplementedException();
    }
  }
}
