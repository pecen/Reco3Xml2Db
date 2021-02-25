using Reco3Xml2Db.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal {
  public interface IVehicleDal {
    List<VehicleDto> Fetch();
    VehicleDto Fetch(int vehicleId);
    VehicleDto Fetch(string vin);

    void Delete(int vehicleId);
    void DeleteOnGroupId(int groupId);
  }
}
