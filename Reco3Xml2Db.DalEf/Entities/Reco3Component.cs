using System;
using System.ComponentModel.DataAnnotations;

namespace Reco3Xml2Db.DalEf.Entities {
  public class Reco3Component {
    [Key]
    public int ComponentId { get; set; }
    public string PDNumber { get; set; }
    public DateTime DownloadedTimestamp { get; set; }
    public string Description { get; set; }
    public int PD_Status { get; set; }
    public int Component_Type { get; set; }
    public string XML { get; set; }
    public int PD_Source { get; set; }
    public int? SourceComponentId { get; set; }
  }
}
