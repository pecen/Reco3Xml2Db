using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.DalEf.Entities {
  public class Vehicle {
    [Key]
    public int VehicleId { get; set; }
    public string VIN { get; set; }
    public string XML { get; set; }
    public int Vehicle_Mode { get; set; }
    public int GroupId { get; set; }
  }
}
