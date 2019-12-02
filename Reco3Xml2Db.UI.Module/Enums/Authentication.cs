using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum Authentication {
    [Description("Sql Server")]
    SQLServer,
    Windows,
    //[Description("Active Directory")]
    //ActiveDirectory
  }
}
