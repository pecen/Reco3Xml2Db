using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal.Dto {
  public class VehicleDto {
    public int VehicleId { get; set; }
    public string VIN { get; set; }
    public string Xml { get; set; }
    public int VehicleMode { get; set; }
    public int GroupId { get; set; }
  }
}
