using System.ComponentModel;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum ComponentType {
    Engine = 1,
    Gearbox = 2,
    Axle = 3,
    Retarder = 4,
    Tyre = 5,
    Airdrag = 6,
    [Description("Torque Converter")]
    TorqueConverter = 7,
    Unknown = 0
  }
}
