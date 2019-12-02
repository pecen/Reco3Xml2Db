using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal.Dto {
  public class ComponentDto {
    public int ComponentId { get; set; }
    public string PDNumber { get; set; }
    public DateTime DownloadedTimestamp { get; set; }
    public string Description { get; set; }
    public int PDStatus { get; set; }
    public int ComponentType { get; set; }
    public string Xml { get; set; }
    public int PDSource { get; set; }
    public int? SourceComponentId { get; set; }
  }
}
