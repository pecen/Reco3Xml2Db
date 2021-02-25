using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum PDStatus {
    Released = 1,
    [Description("In Work")]
    InWork = 2,
    Unknown = 0
  }
}
