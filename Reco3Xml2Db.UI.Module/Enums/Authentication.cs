using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum Authentication {
    [Description("Sql Server")]
    SQLServer,
    Windows,
    [Description("Amazon Web Services")]
    AWS
  }
}
