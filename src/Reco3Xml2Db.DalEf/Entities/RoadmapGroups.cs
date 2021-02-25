using System;
using System.ComponentModel.DataAnnotations;

namespace Reco3Xml2Db.DalEf.Entities {
  public class RoadmapGroups {
    [Key]
    public int RoadmapGroupId { get; set; }
    public string OwnerSss { get; set; }
    public string RoadmapName { get; set; }
    public bool Protected { get; set; }
    public DateTime CreationTime { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public string XML { get; set; }
    public int Validation_Status { get; set; }
    public int ConvertToVehicleInput_Status { get; set; }
  }
}
