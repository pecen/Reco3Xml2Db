using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reco3Xml2Db.UI.Module.Enums {
  public enum ValidationStatus {
    Pending,
    Processing,
    [Description("Validated with success")]
    ValidatedWithSuccess,
    [Description("Validated with failure")]
    ValidatedWithFailures
  }
}
