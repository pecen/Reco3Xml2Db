using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum ConnectionStringItems {
    [Description("Data Source")]
    DataSource,
    [Description("Initial Catalog")]
    InitialCatalog,
    [Description("Integrated Security")]
    IntegratedSecurity,
    [Description("User ID")]
    UserId,
    Password,
    [Description("Persist Security Info")]
    PersistSecurityInfo
  }
}
