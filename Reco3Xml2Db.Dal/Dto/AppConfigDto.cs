using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.Dal.Dto {
  public class AppConfigDto {
    public string Server { get; set; }
    public string Database { get; set; }
    public int Authentication { get; set; }
    public string XmlFilePath { get; set; }
  }
}
