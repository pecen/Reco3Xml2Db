using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal.Dto {
  public class RoadmapGroupDto {
    public int RoadmapGroupId { get; set; }
    public string OwnerSss { get; set; }
    public string RoadmapName { get; set; }
    public bool Protected { get; set; }
    public DateTime CreationTime { get; set; }
    public string Xml { get; set; }
    public int ValidationStatus { get; set; }
    public int ConvertToVehicleInputStatus { get; set; }
  }
}
