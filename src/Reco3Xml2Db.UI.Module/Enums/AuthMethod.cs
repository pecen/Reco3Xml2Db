using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum AuthMethod {
    [Description("Sql Server")]
    SQLServer,
    Windows,
    [Description("Amazon Web Services")]
    AWS
  }
}
